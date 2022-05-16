using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float h;
    float v;

    bool _moveSwitch = true;
    bool _rollSwitch = false;

    Vector3 _dir;
    Vector3 _latestPos;

    Rigidbody _rb;

    Animator _anim;

    //�ړ����ɉ�����͂Ƒ��x�̍ő�l
    [SerializeField] float _movePower;
    [SerializeField] float _moveMaxSpeed;

    //�v���C���[���_���f���J����
    [SerializeField] Camera _playerCam;

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

        _dir = (Vector3.forward * v + Vector3.right * h).normalized;
        _dir = _playerCam.transform.TransformDirection(_dir);

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

        if (h == 0 && v == 0)
        {
            _anim.SetBool("Move", false);
        }
        else
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

        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("A_Run"))
        {
            if (Input.GetButtonDown("Slide"))
            {
                _anim.SetTrigger("Slide");
            }
        }
    }

    //���[�����O�\�s�\�̐؂�ւ�
    void RollSwitch()
    {
        //�O�����Z�q��true�̎���false�Afalse�̎���true�ɕς���
        _rollSwitch = _rollSwitch ? _rollSwitch = false : _rollSwitch = true;
        Debug.Log(_rollSwitch);
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
    }

}
