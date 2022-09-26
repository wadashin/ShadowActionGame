using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBallScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * 0.1f;
    }

    IEnumerator Dath()
    {
        yield return new WaitForSecondsRealtime(6);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Enemy"))
        {
            //if(TryGetComponent(out PlayerManagement playerManagement))
            //{
            //    Debug.Log("Ç±Ç±óvïœçX");
            //    playerManagement.Damage(10);
            //}
            //else
            //{
            //    Debug.Log(1);
            //}
            Destroy(this.gameObject);
        }
    }
}
