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

    [SerializeField] ParticleSystem _guardParticle;

    [SerializeField] ParticleSystem _breakParticle;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if(_operation)
        {
            transform.LookAt(new Vector3(_gameManager.Player.transform.position.x, _gameManager.Player.transform.position.y, _gameManager.Player.transform.position.z));
        }
    }

    public void WakeUp()
    {
        _operation = true;
        StartCoroutine("Attack");
    }

    /// <summary>
    /// プレイヤーから攻撃を受けた際に呼ばれる関数　　グーグル先生曰く攻撃を受けるって意味らしい
    /// </summary>
    public void BeAttacked()
    {
        StopAllCoroutines();
        StartCoroutine("Attack");
    }

    /// <summary>
    /// ガードした時に呼ばれる関数
    /// </summary>
    public void Guard()
    {
        _guardParticle.Play();
    }

    /// <summary>
    /// ガードブレイクされた時に呼ばれる関数
    /// </summary>
    public void BreakGuard()
    {
        _breakParticle.Play();
    }


    IEnumerator Attack()
    {
        yield return new WaitForSecondsRealtime(Random.Range(7,12));
        StartCoroutine("RapidFire");
        StartCoroutine("Attack");
    }

    IEnumerator RapidFire()
    {
        for (int i = 0; i < _rapidNumber; i++)
        {
            Instantiate(_energyBall, _attackPoint.position, this.transform.rotation);
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}
