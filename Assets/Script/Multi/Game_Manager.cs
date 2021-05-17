using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class Game_Manager : MonoBehaviour
{

    PhotonView PV;
    GameObject[] listmonstre;
    GameObject[] listloot;
    int numloot;
    int nummonstre;
    public int Delay;
    bool Attendre;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnGame();
        }
    }

    void Update()
    {
        listmonstre = GameObject.FindGameObjectsWithTag("test");
        nummonstre = listmonstre.Length;
        if (nummonstre < SpawnManager.Instance.spawnpoint.Length && !Attendre)
        {
            StartCoroutine(SpawnDelay());
        }
        foreach (GameObject ennemi in listmonstre)
        {
            if (ennemi.GetComponent<EnnemyAI>().HealthPoint <= 0)
            {
                if (ennemi.GetComponent<EnnemyAI>().lootdrop != null)
                {
                    PhotonNetwork.Instantiate(ennemi.GetComponent<EnnemyAI>().lootdrop, ennemi.GetComponent<EnnemyAI>().loot_a.transform.position, Quaternion.identity);
                    ennemi.GetComponent<EnnemyAI>().lootdrop = null;
                }
                if (!ennemi.GetComponent<EnnemyAI>().isDead)
                {
                    //FIXME --> Il faut ajouter les stats ajouté
                    ennemi.GetComponent<PhotonView>().RPC("Dead", RpcTarget.All);
                    ennemi.GetComponent<EnnemyAI>().Target.GetComponent<Joueur>().Money += Random.Range(1, 100);
                    ennemi.GetComponent<EnnemyAI>().Target.GetComponent<Joueur>().Experience += 10;
                    //ennemi.GetComponent<EnnemyAI>().Target.GetComponent<Joueur>());
                }
            }
            
        }
        listloot = GameObject.FindGameObjectsWithTag("Loot");
        numloot = listloot.Length;
        if (numloot > 0)
        {
            foreach(GameObject loot in listloot)
            {
                if (loot.GetComponent<Loot>().inter_ == true)
                {
                    loot.GetComponent<PhotonView>().RPC("detruire", RpcTarget.All);
                    loot.GetComponent<Loot>().inter_ = false;
                }
            }
        }
    }

    public void SpawnGame()
    {
        foreach (SpawnPoint spawner in SpawnManager.Instance.GetComponentsInChildren<SpawnPoint>())
        {
            PhotonNetwork.Instantiate("Aspirhanoi MULTI", spawner.transform.position, Quaternion.identity);
        }

    }

    public void RespawnCreature()
    {
        Transform spawnpoint = SpawnManager.Instance.GetSpawnPoint();
        PhotonNetwork.Instantiate("Aspirhanoi MULTI", spawnpoint.position, Quaternion.identity);
    }

    public IEnumerator SpawnDelay()  // Pour faire attendre 
    {
        Attendre = true;
        yield return new WaitForSeconds(Delay);
        RespawnCreature();
        Attendre = false;
    }
}