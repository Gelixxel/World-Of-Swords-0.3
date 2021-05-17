using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public CheckPointChar cp;
    public int HP;
    public int HPmax;
    public int Attack;
    public bool Isinvinsible = false;
    public Renderer rend;
    public bool IsDead;
    

    public void Start()
    {
        HPmax = HP;
        IsDead = false;
        cp.Lastpoint = transform.position;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Hurt" && !Isinvinsible)
        {
            
            if (HP <= 0)
            {
                IsDead = true;
                print("YOU ARE DEAD");
                SetHealth(HPmax);
            }
            else
            {
                print("Aie");
                Isinvinsible = true;
                StartCoroutine("ResetInvincible");
            }
        }
    }

    public void SetHealth(int val)
    {
        if (HP > HPmax)
        {
            HP = HPmax;
        }

        if (HP <= 0)
        {
            cp.Respawn();
            HP = HPmax;
            IsDead = false;
        }
    }
    

    IEnumerator ResetInvincible()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(.2f);
            rend.enabled = !rend.enabled;
        }
        yield return new WaitForSeconds(.2f);
        rend.enabled = true;
        Isinvinsible = false;
    }
}
