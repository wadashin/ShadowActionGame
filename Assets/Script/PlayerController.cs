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

        _dir = (Vector3.forward * v + Vector3.right * h).normalized;
        _dir = _playerCam.transform.TransformDirection(_dir);

        //前回からどこに進んだかをベクトルで取得
        Vector3 diff = transform.position - _latestPos;
        //横にしか回転しないようにする
        diff = new Vector3(diff.x, 0, diff.z);
        //前回のPositionの更新
        _latestPos = transform.position;

        //ベクトルの大きさが0.01以上の時に向きを変える処理をし走りアニメーションを再生する
        if (diff.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(diff); //向きを変更する
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

    //アニメーション遷移の管理
    void AnimControlMethod()
    {
        //移動系
        _anim.SetFloat("Speed", _rb.velocity.magnitude);

        if (h == 0 && v == 0)
        {
            _anim.SetBool("Move", false);
        }
        else
        {
            _anim.SetBool("Move", true);
        }


        //ローリング
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

    //ローリング可能不可能の切り替え
    void RollSwitch()
    {
        //三項演算子でtrueの時はfalse、falseの時はtrueに変える
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
