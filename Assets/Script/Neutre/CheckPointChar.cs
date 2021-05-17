using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointChar : MonoBehaviour
{
    public Vector3 Lastpoint;
    //public GameObject lvl;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "checkpoint")
        {
            Lastpoint = transform.position;
            //GameObject lv = Instantiate(lvl, transform.position, Quaternion.identity);
            //Destroy(lv,2f);
        }
    }

    public void Respawn()
    {
        transform.position = Lastpoint;
    }

   /*IEnumerator Resetlvl()
    {
        yield return WaitForSeconds(2f);
    }*/
}
