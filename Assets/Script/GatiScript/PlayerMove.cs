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
        ////////////////////停止時と接地時の移動速度の切り替え////////////////////
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
        //接地判定のレイの発射位置をplayerの位置にする
        rayPos = transform.position;// + new Vector3(0,-0.1f,0);

        //接地判定のレイを下に向ける
        Ray ray = new Ray(rayPos, Vector3.down);

        //接地判定
        _onPlaceSwitch = Physics.Raycast(ray, playerManagement.GroundDistance);


        Debug.DrawRay(rayPos, Vector3.down * playerManagement.GroundDistance, Color.red);
    }
}
