using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tanmatu : MonoBehaviour
{
    GameObject _skillTree;

    void Start()
    {
        _skillTree = GameObject.Find("SkillTreeCanvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            _skillTree.SetActive(true);
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            _skillTree.SetActive(false);
        }
    }
}
