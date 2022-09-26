using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject _door;

    [SerializeField] List<EnemyManager> _enemys;

    void Start()
    {
        foreach(EnemyManager enemy in _enemys)
        {
            enemy._myRoom = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemysGain(EnemyManager gainEnemy)
    {
        _enemys.Remove(gainEnemy);
        if(_enemys.Count <= 0)
        {
            Destroy(_door);
        }
    }

    public void EnemysWakeUp()
    {
        foreach (EnemyManager enemy in _enemys)
        {
            enemy._enemyScript.WakeUp();
        }
    }
}
