using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float h;
    float v;
    bool _moveSwitch = true;

    Vector3 _dir;
    Vector3 _latestPos;

    Rigidbody _rb;

    //�ړ����ɉ�����͂Ƒ��x�̍ő�l
    [SerializeField] float _movePower;
    [SerializeField] float _moveMaxSpeed;

    //�v���C���[���_���f���J����
    [SerializeField] Camera _playerCam;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        _dir = Vector3.forward * v + Vector3.right * h;
        _dir = _playerCam.transform.TransformDirection(_dir);

        //Vector3 diff = transform.position - _latestPos;   //�O�񂩂�ǂ��ɐi�񂾂����x�N�g���Ŏ擾
        //_latestPos = transform.position;  //�O���Position�̍X�V



        //    //transform.rotation = Quaternion.LookRotation(diff); //������ύX����
        //    transform.eulerAngles = new Vector3(diff.x, 0, diff.z);


        Vector3 diff = transform.position - _latestPos;   //�O�񂩂�ǂ��ɐi�񂾂����x�N�g���Ŏ擾
        _latestPos = transform.position;  //�O���Position�̍X�V

        //�x�N�g���̑傫����0.01�ȏ�̎��Ɍ�����ς��鏈����������A�j���[�V�������Đ�����
        if (diff.magnitude > 0.01f)
        {
            //transform.rotation = Quaternion.LookRotation(diff); //������ύX����
            //transform.rotation = new Quaternion(diff.x,0,diff.z,transform.rotation.w);
            transform.eulerAngles = new Vector3(diff.x, diff.y, diff.z);
        }
    }

    private void FixedUpdate()
    {
        if(_moveSwitch)
        {
            if (_rb.velocity.magnitude < _moveMaxSpeed)
            {
                _rb.AddForce(_dir.normalized * _movePower, ForceMode.Force);

                
            }
        }
    }
}
