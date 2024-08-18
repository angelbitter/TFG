using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController instance;
    public float waitForRespawn;
    public int points;
    public int coinsCollected;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        points = 0;
    }
    
    public void Respawn()
    {
        StartCoroutine(RespawnCoroutine());
    }
    
    IEnumerator RespawnCoroutine()
    {
        Baqueta_movement.instance.DisableBaqueta();
        yield return new WaitForSeconds(waitForRespawn);
        Baqueta_movement.instance.EnableBaqueta();
        Baqueta_movement.instance.transform.position = RespawnController.instance.respawnPoint;
        Baqueta_movement.instance.OnRespawn();
    }
}
