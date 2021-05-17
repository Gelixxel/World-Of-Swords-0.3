using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool IsDiscovered = false;
    public Vector3 dir;
    
    public GameObject checkpoint;
    public GameObject effect;
    public Material crystal;
    private MeshRenderer rend;

    void Start()
    {
        rend = GetComponent<MeshRenderer>();
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !IsDiscovered)
        {
            IsDiscovered = true;
            AnimCrystal();
        }
    }

    public void AnimCrystal()
    {
        if (IsDiscovered)
        {
            Instantiate(checkpoint, transform.position, Quaternion.identity);
            rend.material = crystal;
            GameObject effet = Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(effet,2f);
        }
    }

    void Update()
    {
        if (IsDiscovered)
        {
            transform.Rotate(dir * Time.deltaTime);
        }
    }

    
}
