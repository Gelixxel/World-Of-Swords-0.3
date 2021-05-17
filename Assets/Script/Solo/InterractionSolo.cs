using System;
using UnityEngine;

public class InteractionSolo : MonoBehaviour
{
    public bool IsFocus = false;
    public JoueurSolo player;
    public bool interraction = false;

    public void Focus(JoueurSolo playerTarget)
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

    public virtual void Interract(JoueurSolo player)
    {
        Debug.Log("On interragie avec "+ transform.name);
    }
}
