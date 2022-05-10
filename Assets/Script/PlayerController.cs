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

    Animator _anim;

    //移動時に加える力と速度の最大値
    [SerializeField] float _movePower;
    [SerializeField] float _moveMaxSpeed;

    //プレイヤー視点を映すカメラ
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

        _dir = (Vector3.forward * v + Vector3.right * h ).normalized;
        _dir = _playerCam.transform.TransformDirection(_dir);


        //Vector3 diff = transform.position - _latestPos;   //前回からどこに進んだかをベクトルで取得
        //_latestPos = transform.position;  //前回のPositionの更新



        //    //transform.rotation = Quaternion.LookRotation(diff); //向きを変更する
        //    transform.eulerAngles = new Vector3(diff.x, 0, diff.z);


        Vector3 diff = transform.position - _latestPos;   //前回からどこに進んだかをベクトルで取得
        _latestPos = transform.position;  //前回のPositionの更新

        //ベクトルの大きさが0.01以上の時に向きを変える処理をし走りアニメーションを再生する
        if (diff.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(diff); //向きを変更する
            //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            //transform.rotation = new Quaternion(diff.x,0,diff.z,transform.rotation.w);
            //transform.eulerAngles = new Vector3(diff.x, diff.y, diff.z);
        }

        //アニメーション管理の関数
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

    void AnimControlMethod()
    {
        _anim.SetFloat("Speed", _rb.velocity.magnitude);

        if (h == 0 && v == 0)
        {
            _anim.SetBool("Walk", false);
        }
        else
        {
            _anim.SetBool("Walk", true);
        }
    }
}
