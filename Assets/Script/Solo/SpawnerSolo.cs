using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnerSolo : MonoBehaviour
{
    public int MaxDeEntités;
    public EnnemyAISolo Entité;
    public int Delay;
    public Transform PlacementDeSpawn;
    public bool Attendre;
    public int spawn;

    void Update()
    {
        if (spawn < MaxDeEntités && !Attendre)
        {
            StartCoroutine(SpawnDelay()); //Lance de Delay
        }
    }
    
    public IEnumerator SpawnDelay()  // Pour faire attendre 
    {
        Debug.Log("On passe par là");
        Attendre = true;
        yield return new WaitForSeconds(Delay);
        EnnemyAISolo neew =Instantiate(Entité, PlacementDeSpawn.position, Quaternion.identity);
        neew.spawner = this;
        spawn++;
        Attendre = false;
    }
}
