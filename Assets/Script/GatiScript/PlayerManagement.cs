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
