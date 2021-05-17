using System;
using UnityEngine;
using Photon.Pun;

public class Interaction : MonoBehaviourPun
{
    public bool IsFocus = false;
    public Joueur player;
    public bool interraction = false;

    public void Focus(Joueur playerTarget)
    {
        IsFocus = true;
        player = playerTarget;
    }

    public void PlusFocus()
    {
        IsFocus = false;
        player = null;
        interraction = false;
    }

    public virtual void Interract(Joueur player)
    {
        Debug.Log("On interragie avec "+ transform.name);
    }
}
