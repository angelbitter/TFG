using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController instance;
    public float waitForRespawn;

    private void Awake()
    {
        instance = this;
    }
    public void Respawn()
    {
        StartCoroutine(RespawnCoroutine());
    }
    
    IEnumerator RespawnCoroutine()
    {
        Baqueta_movement.instance.gameObject.SetActive(false);
        yield return new WaitForSeconds(waitForRespawn);
        Baqueta_movement.instance.gameObject.SetActive(true);
        Baqueta_movement.instance.transform.position = RespawnController.instance.respawnPoint;
    }
}
