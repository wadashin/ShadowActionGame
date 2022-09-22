using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Tooltip("基本180")]
    [SerializeField]
    float _movePower;

    [Tooltip("基本15")]
    [SerializeField]
    float _moveMaxSpeed;

    [Tooltip("基本100")]
    [SerializeField]
    float _maxEnemyHp;
    float _enemyHp = 0;

    [Tooltip("基本100")]
    [SerializeField]
    float _maxEnemyShield;
    float _enemyShield;

    EnemyScript _enemyScript;

    public RoomScript _myRoom;


    public float MovePower
    {
        get { return _movePower; }
    }
    public float MoveMaxSpeed
    {
        get { return _moveMaxSpeed; }
    }
    public float Hp
    {
        get { return _enemyHp; }
        set { _enemyHp = value;}
    }
    public float Shield
    {
        get { return _enemyShield; }
        set { _enemyShield = value; }
    }

    void Start()
    {
        Hp = _maxEnemyHp;
        Shield = _maxEnemyShield;
        _enemyScript = GetComponent<EnemyScript>();
    }

    public void Damage(int power)
    {
        _enemyScript.BeAttacked();//攻撃間隔のリセット
        if (Shield <= 0)
        {
            Hit(power);
        }
        else
        {
            Guard(power);
        }
    }

    public void Guard(int power)
    {
        Shield -= power;
        if(Shield > 0)
        {
            _enemyScript.Guard();
        }
        else
        {
            _enemyScript.BreakGuard();
        }
    }

    public void Hit(int power)
    {
        Hp -= power;
        if(Hp <= 0)
        {
            Defeat();
        }
    }

    public void Defeat()
    {
        Debug.Log(1);
        _myRoom.EnemysGain(this);
        _enemyScript.StopAllCoroutines();
    }

}
