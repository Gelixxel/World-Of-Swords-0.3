using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCount : MonoBehaviour
{
    public static PlayerCount Instance;
    public int playercount = 0;

    void Update()
    {
        playercount = transform.childCount;      
    }
}
