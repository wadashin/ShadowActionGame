using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int _stageNum = 0;

    EnemyScript[] enemyScripts;

    GameObject player = default;


    public GameObject Player
    {
        get { return player; }
        set { player = value; }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
