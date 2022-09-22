using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
