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
    //接地判定のレイの発射位置
    Vector3 rayPos;

    //接地判定のレイ
    //Ray ray;

    Rigidbody _rb;

    Animator _anim;

    //移動時に加える力と速度の最大値
    [SerializeField] float _movePower;
    [SerializeField] float _moveMaxSpeed;

    //プレイヤー視点を映すカメラ
    [SerializeField] Camera _playerCam;

    //接地判定の距離
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
        //接地判定の関数
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

    //アニメーション遷移の管理
    void AnimControlMethod()
    {
        //移動系
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


        //ローリング
        if (_rollSwitch)
        {
            if (Input.GetButtonDown("Roll"))
            {
                _anim.SetTrigger("Roll");
            }
        }

        //スライディング
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("A_Run") && !_anim.IsInTransition(0))
        {
            if (Input.GetButtonDown("Slide"))
            {
                _anim.SetTrigger("Slide");
            }
        }

        //ジャンプ
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
        //接地判定のレイの発射位置をplayerの位置にする
        rayPos = transform.position;// + new Vector3(0,-0.1f,0);

        //接地判定のレイを下に向ける
        Ray ray = new Ray(rayPos, Vector3.down);

        //接地判定
        _onPlaceSwitch = Physics.Raycast(ray, _distance);


        Debug.DrawRay(rayPos, Vector3.down * _distance, Color.red);
    }

    //ローリング可能不可能の切り替え
    void RollSwitch()
    {
        //三項演算子でtrueの時はfalse、falseの時はtrueに変える
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
