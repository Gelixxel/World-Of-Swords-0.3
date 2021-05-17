using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int MaxDeEntités;
    public string NomEntité;
    public List<GameObject> EntitésActuelle;
    public int Delay;
    public Transform PlacementDeSpawn;
    public bool Attendre;

    void Update()
    {
        if (EntitésActuelle.Count < MaxDeEntités && !Attendre)
        {
            StartCoroutine(SpawnDelay()); //Lance de Delay
        }

        foreach (var entité in EntitésActuelle)
        {
            if (entité == null)
            {
                EntitésActuelle.Remove(entité); //Remove des entités mortes / Loot
            }
        }
    }
    
    public IEnumerator SpawnDelay()  // Pour faire attendre 
    {
        Debug.Log("On passe par là");
        Attendre = true;
        yield return new WaitForSeconds(Delay);
        EntitésActuelle.Add(PhotonNetwork.Instantiate(NomEntité, PlacementDeSpawn.position, Quaternion.identity));
        Attendre = false;
    }
}
