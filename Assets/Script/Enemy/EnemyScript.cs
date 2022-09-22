using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    GameManager _gameManager;

    bool _operation = false;

    [SerializeField] int _rapidNumber;

    [SerializeField] Transform _attackPoint;

    [SerializeField] GameObject _energyBall;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        WakeUp();
    }

    void Update()
    {
        if(_operation)
        {
            transform.LookAt(new Vector3(_gameManager.Player.transform.position.x, _gameManager.Player.transform.position.y, _gameManager.Player.transform.position.z));
        }
    }

    void WakeUp()
    {
        Debug.Log(1);
        _operation = true;
        StartCoroutine("Attack");
    }

    IEnumerator Attack()
    {
        Debug.Log(2);
        yield return new WaitForSecondsRealtime(11);
        StartCoroutine("RapidFire");
        StartCoroutine("Attack");
    }

    IEnumerator RapidFire()
    {
        Debug.Log(3);
        for (int i = 0; i < _rapidNumber; i++)
        {
            Instantiate(_energyBall, _attackPoint.position, this.transform.rotation);
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}
