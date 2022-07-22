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


    Vector3 _dir;
    Vector3 rayPos;
    Vector3 _latestPos;
    Rigidbody _rb;

    PlayerManagement playerManagement;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        playerManagement = GetComponent<PlayerManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        ////////////////////��~���Ɛڒn���̈ړ����x�̐؂�ւ�////////////////////
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        if (h == 0 && v == 0)
        {
            _dir = (Vector3.forward * v / 2 + Vector3.right * h / 2).normalized;
            _dir = playerManagement.PlayerCam.transform.TransformDirection(_dir);
        }
        //else if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        //{
        //    _dir = (Vector3.forward * v / 10 + Vector3.right * h / 10).normalized;
        //    _dir = _playerCam.transform.TransformDirection(_dir);
        //}
        else if (_onPlaceSwitch)
        {
            _dir = (Vector3.forward * v + Vector3.right * h).normalized;
            _dir = playerManagement.PlayerCam.transform.TransformDirection(_dir);
        }
        else
        {
            _dir = (Vector3.forward * v / 10 + Vector3.right * h / 10).normalized;
            _dir = playerManagement.PlayerCam.transform.TransformDirection(_dir);
        }


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
}
