using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Tooltip("��{180")]
    [SerializeField]
    float _movePower;

    [Tooltip("��{15")]
    [SerializeField]
    float _moveMaxSpeed;

    [Tooltip("��{100")]
    [SerializeField]
    float _maxEnemyHp;
    float _enemyHp = 0;

    [Tooltip("��{100")]
    [SerializeField]
    float _maxEnemyShield;
    float _enemyShield;


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
    }

    public void Damage(int power)
    {
        if(Shield <= 0)
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
    }

    public void Hit(int power)
    {
        Hp -= power;
    }

}
