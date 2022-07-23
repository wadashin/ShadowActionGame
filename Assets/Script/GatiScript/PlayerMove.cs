using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update

    float h;
    float v;

    bool _onPlaceSwitch;
    bool _moveSwitch = true;
    bool _rollSwitch = true;
    bool _jumpSwitch = true;

    bool _idling = true;

    Vector3 _dir;
    Vector3 rayPos;
    Vector3 _latestPos;
    Rigidbody _rb;
    Animator _anim;

    PlayerManagement playerManagement;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        playerManagement = GetComponent<PlayerManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        ////////////////////��~���Ɛڒn���̈ړ����x�̐؂�ւ�////////////////////
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        //if (h == 0 && v == 0)
        //{
        //    _dir = (Vector3.forward * v / 2 + Vector3.right * h / 2).normalized;
        //    _dir = playerManagement.PlayerCam.transform.TransformDirection(_dir);
        //}
        //else if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        //{
        //    _dir = (Vector3.forward * v / 10 + Vector3.right * h / 10).normalized;
        //    _dir = _playerCam.transform.TransformDirection(_dir);
        //}

        //if (_onPlaceSwitch)
        //{
            _dir = (Vector3.forward * v + Vector3.right * h).normalized;
            _dir = playerManagement.PlayerCam.transform.TransformDirection(_dir);
        //}

        //else
        //{
        //    _dir = (Vector3.forward * v / 10 + Vector3.right * h / 10).normalized;
        //    _dir = playerManagement.PlayerCam.transform.TransformDirection(_dir);
        //}


        ////////////////////�ړ��ɉ����������̕ύX�@�\////////////////////
        //�O�񂩂�ǂ��ɐi�񂾂����x�N�g���Ŏ擾
        Vector3 diff = transform.position - _latestPos;
        //���ɂ�����]���Ȃ��悤�ɂ���
        diff = new Vector3(diff.x, 0, diff.z);
        //�O���Position�̍X�V
        _latestPos = transform.position;
        if (diff.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(diff); //������ύX����
        }

        ////////////////////�֐��Q////////////////////
        OnPlace();

        AnimControlMethod();
    }

    private void FixedUpdate()
    {
        if (_moveSwitch)
        {
            if (_rb.velocity.magnitude < playerManagement.MoveMaxSpeed)
            {
                _rb.AddForce(_dir.normalized * playerManagement.MovePower, ForceMode.Force);
            }
        }
    }

    void OnPlace()
    {
        //�ڒn����̃��C�̔��ˈʒu��player�̈ʒu�ɂ���
        rayPos = transform.position;// + new Vector3(0,-0.1f,0);

        //�ڒn����̃��C�����Ɍ�����
        Ray ray = new Ray(rayPos, Vector3.down);

        //�ڒn����
        _onPlaceSwitch = Physics.Raycast(ray, playerManagement.GroundDistance);


        Debug.DrawRay(rayPos, Vector3.down * playerManagement.GroundDistance, Color.red);
    }

    void AnimControlMethod()
    {
        Debug.Log(_idling);
        if(_idling)
        {
            if (Mathf.Abs(h) + Mathf.Abs(v) > 0.04f)
            {
                playerManagement.animationCtrl.Play("A_Run");
            }
            else
            {
                playerManagement.animationCtrl.Play("A_idle");
            }
        }
        

        if (Input.GetButtonDown("Roll"))
        {
            _idling = false;
            playerManagement.animationCtrl.Play("A_roll_front");
            playerManagement.animationCtrl.SetPlaybackDelegate(Sawadanosio);
        }


        //�ړ��n
        //_anim.SetFloat("Speed", _rb.velocity.magnitude);

        //if (h == 0 && v == 0)
        //{
        //    _anim.SetBool("Move", false);
        //}
        //else if (_anim.GetCurrentAnimatorStateInfo(0).IsName("A_idle"))
        //{
        //    _anim.SetBool("Move", true);
        //}


        ////////////////////////////////////////////////////////////////////////�v���P
        ////���[�����O
        //if (Input.GetButtonDown("Roll"))
        //{
        //    if (_anim.GetCurrentAnimatorStateInfo(0).IsName("A_idle") || _anim.GetCurrentAnimatorStateInfo(0).IsName("A_Run"))
        //    {
        //        _anim.SetTrigger("Roll");
        //    }
        //}

        ////�X���C�f�B���O
        //if (_anim.GetCurrentAnimatorStateInfo(0).IsName("A_Run") && !_anim.IsInTransition(0))
        //{
        //    if (Input.GetButtonDown("Slide"))
        //    {
        //        _anim.SetTrigger("Slide");
        //    }
        //}



        ////�W�����v
        //if (_jumpSwitch && _onPlaceSwitch)
        //{
        //    if (Input.GetButtonDown("Jump"))
        //    {
        //        _anim.SetTrigger("Jump");
        //    }
        //}
        //if (_onPlaceSwitch)
        //{
        //    _anim.SetBool("OnPlace", true);
        //}
        //else
        //{
        //    _anim.SetBool("OnPlace", false);
        //}

    }
    void Sawadanosio()
    {
        _idling = false;
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
}
