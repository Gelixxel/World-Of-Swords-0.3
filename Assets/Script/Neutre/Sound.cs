using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{

    public AudioSource AudioSource;

    public AudioClip LVLUP;

    public static Sound sound;
    private void Awake()
    {
        if (sound != null && sound != this)
        {
            Destroy(this.gameObject);
            return;
        }
        sound = this;
        DontDestroyOnLoad(this);
    }
}
