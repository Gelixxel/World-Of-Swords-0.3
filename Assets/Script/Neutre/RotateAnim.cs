using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAnim : MonoBehaviour
{
    public Vector3 dir;

    public Checkpoint cp;
    public GameObject checkpoint;
    public Material crystal;

    public void AnimCrystal()
    {
        if (cp.IsDiscovered)
        {
            Instantiate(checkpoint, transform.position, Quaternion.identity);
            transform.Rotate(dir * Time.deltaTime);
            Instantiate(crystal, transform.position, Quaternion.identity);
        }
    }
}
