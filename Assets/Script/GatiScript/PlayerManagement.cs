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

    [Tooltip("���݂̈ړ����x�A��{180")]
    [SerializeField]
    float _movePower;

    [Tooltip("�ړ����x�ő�l�A��{15")]
    [SerializeField]
    float _moveMaxSpeed;

    [Tooltip("�U����")]
    [SerializeField]
    int _AttackPower;

    [Tooltip("�R���{1�̃A�j���[�V�����X�e�[�g����")]
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
    [Tooltip("level�X�^�b�N")]
    float _levelStack;

    [Tooltip("HP�̃X���C�_�[")]
    [SerializeField]
    Slider _hpSlider;
    [Tooltip("Shield�̃X���C�_�[")]
    [SerializeField]
    Slider _shieldSlider;
    [Tooltip("level��image")]
    [SerializeField]
    Image _levelGage;
    [Tooltip("level�X�^�b�N��image")]
    [SerializeField]
    Image _levelStackImage;

    [Tooltip("Player��Canvas")]
    [SerializeField]
    Canvas _playerCanvas;
    [Tooltip("level�����̉��o")]
    [SerializeField]
    GameObject _levelUpStaging;

    [SerializeField]
    ParticleSystem _guardParticle;

    [SerializeField]
    ParticleSystem _breakParticle;

    //�U���͈͂̒��S
    [SerializeField]
    Vector3 _rangeCenter = default;
    //�U���͈͂̔��a
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
        // �M�Y���̕\��
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
    /// �U��������B�A�j���[�V�����C�x���g����Ă�ł�
    /// </summary>
    void PlayerAttack()
    {
        // �U���͈͂ɓ����Ă���R���C�_�[���擾����
        var cols = Physics.OverlapSphere(GetAttackRangeCenter(), _rangeRadius);
        // �e�R���C�_�[�ɑ΂��āA���ꂪ Rigidbody �������Ă�����͂�������
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
            Debug.Log("�����v�ύX");
            Damage(10);
        }
    }
}
