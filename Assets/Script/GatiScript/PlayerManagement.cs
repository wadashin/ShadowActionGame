using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManagement : MonoBehaviour
{
    PlayerMove playerMove;
    PlayerStatus playerStatus;
    GameManager gameManager;
    public AnimationCtrl animationCtrl;

    [SerializeField]
    Animator _animator;

    [SerializeField]
    Camera _playerCam;

    [SerializeField]
    float _distance = 1;

    [Tooltip("現在の移動速度、基本180")]
    [SerializeField]
    float _movePower;

    [Tooltip("移動速度最大値、基本15")]
    [SerializeField]
    float _moveMaxSpeed;

    [Tooltip("攻撃力")]
    [SerializeField]
    int _AttackPower;

    [Tooltip("コンボ1のアニメーションステートたち")]
    [SerializeField]
    string[] _attackStateNames;

    [Tooltip("HP")]
    [SerializeField]
    int _hp;
    [Tooltip("Shield")]
    [SerializeField]
    int _shield;

    [Tooltip("level")]
    float _level;
    [Tooltip("levelスタック")]
    float _levelStack;

    [Tooltip("HPのスライダー")]
    [SerializeField]
    Slider _hpSlider;
    [Tooltip("Shieldのスライダー")]
    [SerializeField]
    Slider _shieldSlider;
    [Tooltip("levelのimage")]
    [SerializeField]
    Image _levelGage;
    [Tooltip("levelスタックのimage")]
    [SerializeField]
    Image _levelStackImage;

    [Tooltip("PlayerのCanvas")]
    [SerializeField]
    Canvas _playerCanvas;
    [Tooltip("level増加の演出")]
    [SerializeField]
    GameObject _levelUpStaging;

    [SerializeField]
    ParticleSystem _guardParticle;

    [SerializeField]
    ParticleSystem _breakParticle;

    //攻撃範囲の中心
    [SerializeField]
    Vector3 _rangeCenter = default;
    //攻撃範囲の半径
    [SerializeField]
    float _rangeRadius = 1f;


    public Camera PlayerCam
    {
        get { return _playerCam; }
        set { _playerCam = value; }
    }

    public float GroundDistance
    {
        get { return _distance; }
    }
    public float MovePower
    {
        get { return _movePower; }
    }
    public float MoveMaxSpeed
    {
        get { return _moveMaxSpeed; }
    }
    public string[] AttackStateStack
    {
        get { return _attackStateNames; }
    }

    public float Level
    {
        get { return _level; }
        set
        {
            _level = value;
            if(_level >= 100)
            {
                Level -= 100;
                LevelStack++;
            }
            _levelGage.fillAmount = _level / 100;
        }
    }
    public float LevelStack
    {
        get { return _levelStack; }
        set
        {
            if (_levelStack < 4)
            {
                _levelStack = value;

                _levelStackImage.fillAmount = _levelStack / 4;
            }
        }
    }

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerStatus = GetComponent<PlayerStatus>();
        animationCtrl = GetComponent<AnimationCtrl>();
    }
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gameManager.Player = this.gameObject;

        _hpSlider.value = _hp;
        _shieldSlider.value = _shield;
    }
    void OnDrawGizmosSelected()
    {
        // ギズモの表示
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetAttackRangeCenter(), _rangeRadius);
    }

    Vector3 GetAttackRangeCenter()
    {
        Vector3 center = this.transform.position + this.transform.forward * _rangeCenter.z
            + this.transform.up * _rangeCenter.y
            + this.transform.right * _rangeCenter.x;
        return center;
    }

    public void Damage(int power)
    {
        if (_shield <= 0)
        {
            Hit(power);
        }
        else
        {
            Guard(power);
        }
    }

    public void Guard(int power)
    {
        _shield -= power;
        _shieldSlider.value = _shield;
        if (_shield > 0)
        {
            Guard();
        }
        else
        {
            BreakGuard();
        }
    }

    public void Hit(int power)
    {
        _hp -= power;
        _hpSlider.value = _hp;
        if (_hp <= 0)
        {

        }
    }

    void Guard()
    {
        _guardParticle.Play();
    }

    void BreakGuard()
    {
        _breakParticle.Play();
    }


    /// <summary>
    /// 攻撃をする。アニメーションイベントから呼んでね
    /// </summary>
    void PlayerAttack()
    {
        // 攻撃範囲に入っているコライダーを取得する
        var cols = Physics.OverlapSphere(GetAttackRangeCenter(), _rangeRadius);
        // 各コライダーに対して、それが Rigidbody を持っていたら力を加える
        foreach (var enemys in cols)
        {
            //Rigidbody rb = c.gameObject.GetComponent<Rigidbody>();
            //EnemyManager enemyManager = enemys.gameObject.GetComponent<EnemyManager>();
            if (enemys.TryGetComponent(out EnemyManager enemyManager))
            {
                //rb.AddForce(this.transform.forward * Vector3.Distance(gameObject.transform.position, c.gameObject.transform.position) * _attackPower, ForceMode.Impulse);
                enemyManager.Damage(_AttackPower);
                Instantiate(_levelUpStaging, _playerCanvas.transform);
                Level += 10;
                StartCoroutine("HitStop");
            }

        }
    }

    IEnumerator HitStop()
    {
        _animator.speed = 0;
        yield return new WaitForSecondsRealtime(0.2f);
        _animator.speed = 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnergyBall"))
        {
            Debug.Log("ここ要変更");
            Damage(10);
        }
    }
}
