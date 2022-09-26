using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int _stageNum = 0;

    [SerializeField] GameObject[] _roomsPrefabs;

    [SerializeField] GameObject[] _roomsPoint;

    GameObject[] _rooms;

    GameObject player = default;

    [SerializeField] GameObject BossRoom;


    int _nowRoom = -1;

    public GameObject Player
    {
        get { return player; }
        set { player = value; }
    }

    void Start()
    {
        //foreach(GameObject roomsPoint in _roomsPoint)
        //{
        //    _rooms[0] = Instantiate(_roomsPrefabs[Random.Range(0, _roomsPrefabs.Length)], transform.position = roomsPoint.transform.position, Quaternion.identity);
        //}

        _rooms = new GameObject[_roomsPoint.Length];

        for (int i = 0; i < _roomsPoint.Length; i++)
        {
            _rooms[i] = Instantiate(_roomsPrefabs[Random.Range(0, _roomsPrefabs.Length)], transform.position = _roomsPoint[i].transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void NextRoom()
    {
        _nowRoom++;

        if (_nowRoom < _roomsPrefabs.Length)
        {
            _rooms[_nowRoom].GetComponent<RoomScript>().EnemysWakeUp();
        }
        else
        {
            //‚±‚±‚Éƒ{ƒX•”‰®‚Ì‹N“®‚ğ‘‚¢‚Ä‚Ë
        }
    }
}
