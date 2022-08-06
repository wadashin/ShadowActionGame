using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
    PlayerMove playerMove;
    PlayerStatus playerStatus;
    public AnimationCtrl animationCtrl;

    [SerializeField]
    Camera _playerCam;

    [SerializeField]
    float _distance = 1;

    [Tooltip("基本180")]
    [SerializeField]
    float _movePower;

    [Tooltip("基本15")]
    [SerializeField]
    float _moveMaxSpeed;

    [Tooltip("コンボ1のアニメーションステートたち")]
    [SerializeField]
    string[] _attackStateNames;


    public Camera PlayerCam
    {
        get { return _playerCam; }
        set { _playerCam = value; }
    }

    public float GroundDistance
    {
        get { return _distance; }
    }
    public float MovePower
    {
        get { return _movePower; }
    }
    public float MoveMaxSpeed
    {
        get { return _moveMaxSpeed; }
    }
    public string[] AttackStateStack
    {
        get { return _attackStateNames; }
    }




    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerStatus = GetComponent<PlayerStatus>();
        animationCtrl = GetComponent<AnimationCtrl>();
    }
    void Start()
    {
        
    }
}
