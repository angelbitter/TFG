using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    public Respawn[] respawns;
    public Vector3 respawnPoint;
    public static RespawnController instance;
    
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        respawns = FindObjectsOfType<Respawn>();
        respawnPoint = Baqueta_movement.instance.transform.position;
    }

    public void SetRespawnPoint(Vector3 point)
    {
        respawnPoint = point;
    }
}
