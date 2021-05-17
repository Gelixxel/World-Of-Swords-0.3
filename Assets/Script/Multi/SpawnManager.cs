using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    public SpawnPoint[] spawnpoint;

    private void Awake()
    {
        Instance = this;
        spawnpoint = GetComponentsInChildren<SpawnPoint>();
    }

    public Transform GetSpawnPoint()
    {
        return spawnpoint[Random.Range(0, spawnpoint.Length)].transform;
    }

}
