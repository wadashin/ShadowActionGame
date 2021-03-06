using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update

    float h;
    float v;

    bool _onPlaceSwitch;
    bool _jumpFin = true;
    bool _moveSwitch = true;
    bool _rollSwitch = true;

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
        ////////////////////横入力と縦入力の取得////////////////////
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        ////////////////////接地時と非接地時かつ落下中の入力と移動の切り替え////////////////////
        if (_onPlaceSwitch)
        {
            _dir = (Vector3.forward * v + Vector3.right * h).normalized;
            _dir = new Vector3(_dir.x, _rb.velocity.y, _dir.z);
            _dir = new Vector3(playerManagement.PlayerCam.transform.TransformDirection(_dir).x,0, playerManagement.PlayerCam.transform.TransformDirection(_dir).z);
        }
        else if (_rb.velocity.y < 0)
        {
            _dir = (Vector3.forward * v + Vector3.right * h).normalized;
            _dir = new Vector3(_dir.x, _rb.velocity.y, _dir.z);
            _dir = playerManagement.PlayerCam.transform.TransformDirection(_dir);
        }

        ////////////////////移動に応じた向きの変更機能////////////////////
        //前回からどこに進んだかをベクトルで取得
        Vector3 diff = transform.position - _latestPos;
        //横にしか回転しないようにする
        diff = new Vector3(diff.x, 0, diff.z);
        //前回のPositionの更新
        _latestPos = transform.position;
        if (diff.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(diff); //向きを変更する
        }

        ////////////////////関数群////////////////////
        OnPlace();

        AnimControlMethod();
    }

    private void FixedUpdate()
    {
        if (_onPlaceSwitch)
        {
            if (_moveSwitch)
            {
                if (_rb.velocity.magnitude < playerManagement.MoveMaxSpeed)
                {
                    _rb.velocity = _dir.normalized * playerManagement.MovePower;
                }
            }
        }
    }

    void OnPlace()
    {
        //接地判定のレイの発射位置をplayerの位置にする
        rayPos = transform.position;/* + new Vector3(0,-0.1f,0)*/;

        //接地判定のレイを下に向ける
        Ray ray = new Ray(rayPos, Vector3.down);

        //接地判定
        _onPlaceSwitch = Physics.Raycast(ray, playerManagement.GroundDistance);

        Debug.DrawRay(rayPos, Vector3.down * playerManagement.GroundDistance, Color.red);

    }

    void AnimControlMethod()
    {
        if (Mathf.Abs(h) + Mathf.Abs(v) > 0.04f)
        {
            if (_moveSwitch && _onPlaceSwitch)
            {
                playerManagement.animationCtrl.Play("A_Run");
            }
        }
        else
        {
            if (_moveSwitch && _onPlaceSwitch)
            {
                playerManagement.animationCtrl.Play("A_idle");
            }
        }

        if (Input.GetButtonDown("Roll"))
        {
            if (_moveSwitch && _onPlaceSwitch)
            {
                _moveSwitch = false;
                //AddForceFront(16);
                playerManagement.animationCtrl.Play2("A_roll_front"/*, 0.1f*/);
                playerManagement.animationCtrl.SetPlaybackDelegate(AnimFinish);
            }
        }


        if (Input.GetButtonDown("Slide"))
        {
            if (_moveSwitch && _onPlaceSwitch)
            {
                _moveSwitch = false;
                //AddForceFront(17);
                playerManagement.animationCtrl.Play("Running Slide");
                playerManagement.animationCtrl.SetPlaybackDelegate(AnimFinish);
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (_onPlaceSwitch && _moveSwitch)
            {
                _moveSwitch = false;
                playerManagement.animationCtrl.Play("Jump");
                playerManagement.animationCtrl.SetPlaybackDelegate(JumpFinish);
            }
        }
    }

    void AttackMethod()
    {
        if(Input.GetButtonDown("Attack1"))
        {
            if (_onPlaceSwitch && _moveSwitch)
            {
                _moveSwitch = false;
                playerManagement.animationCtrl.Play("Jump");
                playerManagement.animationCtrl.SetPlaybackDelegate(JumpFinish);
            }
        }
    }

    /// <summary>
    /// 前方に力を加えるアニメーション用メソッド
    /// </summary>
    /// <param name="x">加える力</param>
    public void AddForceFront(float x)
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(transform.forward * x, ForceMode.Impulse);
    }

    /// <summary>
    /// 上方に力を加えるアニメーション用メソッド
    /// </summary>
    /// <param name="x">加える力</param>
    public void AddForceUp(float x)
    {
        _rb.velocity = new Vector3(_rb.velocity.x / 2, 0, _rb.velocity.z / 2);
        _rb.AddForce(transform.up * x, ForceMode.Impulse);
    }
    void AnimFinish()
    {
        _moveSwitch = true;
    }
    void JumpFinish()
    {
        _moveSwitch = true;
    }


}
