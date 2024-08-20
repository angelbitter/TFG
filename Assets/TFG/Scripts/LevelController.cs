using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public static LevelController instance;
    public float waitForRespawn;
    public int points;
    public int coinsCollected;
    private int timePoints, enemyPoints;
    private float timePassed;
    public TextMeshProUGUI pointsText;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        points = 0;
        timePassed = 0;
        timePoints = 10000;
    }
    
    public void Respawn()
    {
        StartCoroutine(RespawnCoroutine());
    }
    public void LevelComplete()
    {   
        Baqueta_movement.instance.DisableBaqueta();
        Baqueta_movement.instance.isDead = true;
        timePassed = Time.time;
        timePoints = timePoints - (int)timePassed * 10;
        points += Math.Max(0, timePoints);
        points += coinsCollected * 100 + Baqueta_health.instance.health * 1000 + enemyPoints;
        pointsText.text = "Your points: " + points;

        Beat_manager.instance.StopMusic();
        Beat_manager.instance.PlayWinAudio();
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
