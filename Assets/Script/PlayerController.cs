using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float h;
    float v;

    bool _moveSwitch = true;
    bool _rollSwitch = false;
    bool _jumpSwitch = true;
    bool _onPlaceSwitch;
    bool _attackSwitch = true;

    Vector3 _dir;
    Vector3 _latestPos;
    //�ڒn����̃��C�̔��ˈʒu
    Vector3 rayPos;

    //�ڒn����̃��C
    //Ray ray;

    Rigidbody _rb;

    Animator _anim;

    //�ړ����ɉ�����͂Ƒ��x�̍ő�l
    [SerializeField] float _movePower;
    [SerializeField] float _moveMaxSpeed;

    //�v���C���[���_���f���J����
    [SerializeField] Camera _playerCam;

    //�ڒn����̋���
    [SerializeField] float _distance = 1;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        if (h == 0 && v == 0)
        {
            _dir = (Vector3.forward * v / 2 + Vector3.right * h / 2).normalized;
            _dir = _playerCam.transform.TransformDirection(_dir);
        }
        //else if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        //{
        //    _dir = (Vector3.forward * v / 10 + Vector3.right * h / 10).normalized;
        //    _dir = _playerCam.transform.TransformDirection(_dir);
        //}
        else if (_onPlaceSwitch)
        {
            _dir = (Vector3.forward * v + Vector3.right * h).normalized;
            _dir = _playerCam.transform.TransformDirection(_dir);
        }
        else
        {
            _dir = (Vector3.forward * v / 10 + Vector3.right * h / 10).normalized;
            _dir = _playerCam.transform.TransformDirection(_dir);
        }

        //�O�񂩂�ǂ��ɐi�񂾂����x�N�g���Ŏ擾
        Vector3 diff = transform.position - _latestPos;
        //���ɂ�����]���Ȃ��悤�ɂ���
        diff = new Vector3(diff.x, 0, diff.z);
        //�O���Position�̍X�V
        _latestPos = transform.position;

        //�x�N�g���̑傫����0.01�ȏ�̎��Ɍ�����ς��鏈����������A�j���[�V�������Đ�����
        if (diff.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(diff); //������ύX����
        }


        //�A�j���[�V�����Ǘ��̊֐�
        AnimControlMethod();
        //�ڒn����̊֐�
        OnPlace();
    }

    private void FixedUpdate()
    {
        if (_moveSwitch)
        {
            if (_rb.velocity.magnitude < _moveMaxSpeed)
            {
                _rb.AddForce(_dir.normalized * _movePower, ForceMode.Force);
            }
        }
    }

    //�A�j���[�V�����J�ڂ̊Ǘ�
    void AnimControlMethod()
    {
        //�ړ��n
        _anim.SetFloat("Speed", _rb.velocity.magnitude);
        Debug.Log(_rb.velocity.magnitude);

        if (h == 0 && v == 0)
        {
            _anim.SetBool("Move", false);
        }
        else if (_anim.GetCurrentAnimatorStateInfo(0).IsName("A_idle"))
        {
            _anim.SetBool("Move", true);
        }


        //���[�����O
        if (_rollSwitch)
        {
            if (Input.GetButtonDown("Roll"))
            {
                _anim.SetTrigger("Roll");
            }
        }

        //�X���C�f�B���O
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("A_Run") && !_anim.IsInTransition(0))
        {
            if (Input.GetButtonDown("Slide"))
            {
                _anim.SetTrigger("Slide");
            }
        }

        //�W�����v
        if (_jumpSwitch && _onPlaceSwitch)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _anim.SetTrigger("Jump");
            }
        }
        if (_onPlaceSwitch)
        {
            _anim.SetBool("OnPlace", true);
        }
        else
        {
            _anim.SetBool("OnPlace", false);
        }

        if (Input.GetButtonDown("Attack1"))
        {
            //if (_attackSwitch)
            //{
                _anim.SetTrigger("Attack1");
                //_attackSwitch = false;
            //}
        }
    }

    public void AttackTrue()
    {
        _attackSwitch = true;
    }
    private void OnCollisionStay(Collision collision)
    {
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {

            _anim.SetTrigger("OnPlace2");
        }
    }

    void OnPlace()
    {
        //�ڒn����̃��C�̔��ˈʒu��player�̈ʒu�ɂ���
        rayPos = transform.position;// + new Vector3(0,-0.1f,0);

        //�ڒn����̃��C�����Ɍ�����
        Ray ray = new Ray(rayPos, Vector3.down);

        //�ڒn����
        _onPlaceSwitch = Physics.Raycast(ray, _distance);


        Debug.DrawRay(rayPos, Vector3.down * _distance, Color.red);
    }

    //���[�����O�\�s�\�̐؂�ւ�
    void RollSwitch()
    {
        //�O�����Z�q��true�̎���false�Afalse�̎���true�ɕς���
        _rollSwitch = _rollSwitch ? _rollSwitch = false : _rollSwitch = true;
    }

    void On(string x)
    {
        if (x == "Roll")
        {
            _rollSwitch = true;
        }
        else if (x == "Move")
        {
            _moveSwitch = true;
        }
        else if (x == "Jump")
        {
            _jumpSwitch = true;
        }
    }
    void Off(string x)
    {
        if (x == "Roll")
        {
            _rollSwitch = false;
        }
        else if (x == "Move")
        {
            _moveSwitch = false;
        }
        else if (x == "Jump")
        {
            _jumpSwitch = false;
        }
    }

    void AddForseForward(float a)
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(transform.forward * a, ForceMode.Impulse);
    }

    void AddForseUp(float a)
    {
        //_rb.velocity = Vector3.zero;
        _rb.AddForce(transform.up * a, ForceMode.Impulse);
    }
}
