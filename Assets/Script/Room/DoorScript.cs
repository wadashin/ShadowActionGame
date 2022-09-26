using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("===GameManager===").GetComponent<GameManager>();
    }
    void Open()
    {
        gameManager.NextRoom();
        Destroy(this.gameObject);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Open();
        }
    }
}
