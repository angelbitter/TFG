using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// La clase <c>LevelController</c> se encarga de gestionar el nivel
/// </summary>
/// <remarks>
/// Se manejan aspectos como la puntuación del nivel, el respawn de Baqueta y el final del nivel
/// </remarks>
public class LevelController : MonoBehaviour
{
    /// <summary>
    /// La instancia de la clase LevelController
    /// </summary>
    public static LevelController instance;
    /// <summary>
    /// El tiempo que se espera para que Baqueta resucite
    /// </summary>
    public float waitForRespawn;
    /// <summary>
    ///  La puntuación del nivel
    /// </summary>
    public int points;
    /// <summary>
    /// Las monedas recogidas por Baqueta
    /// </summary>
    public int coinsCollected;
    /// <summary>
    /// Todas las monedas del nivel
    /// </summary>
    public Coin_pickup[] totalCoins;
    private int timePoints;
    /// <summary>
    /// Los puntos obtenidos de eliminar enemigos
    /// </summary>
    public int enemyPoints;
    private float timePassed;
    /// <summary>
    /// El texto que muestra la puntuación
    /// </summary>
    public TextMeshProUGUI pointsText;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        points = 0;
        timePassed = 0;
        enemyPoints = 0;
        coinsCollected = 0;
        timePoints = 10000;
    }
    
    /// <summary>
    /// Método Respawn, se encarga de resucitar a Baqueta
    /// </summary>
    public void Respawn()
    {
        StartCoroutine(RespawnCoroutine());
    }
    /// <summary>
    /// Método LevelComplete, se encarga de finalizar el nivel, guardando la puntuación y mostrando la pantalla de fin de nivel
    /// </summary>
    public void LevelComplete()
    {   
        Baqueta_movement.instance.DisableBaqueta();
        Baqueta_movement.instance.isDead = true;
        timePassed = Time.time;
        timePoints = timePoints - (int)timePassed * 10;
        points += Math.Max(0, timePoints);
        points += coinsCollected * 100 + Baqueta_health.instance.health * 1000 + enemyPoints * 200;
        pointsText.text = "Your points: " + points;

        Beat_manager.instance.StopMusic();
        Beat_manager.instance.PlayWinAudio();

        if(PlayerPrefs.HasKey("levelPoints"))
        {
            if(PlayerPrefs.GetInt("levelPoints") < points)
            {
                PlayerPrefs.SetInt("levelPoints", points);
            }
        }else
        {
            PlayerPrefs.SetInt("levelPoints", points);
        }
        if (PlayerPrefs.HasKey("levelCoinsCollected"))
        {
            if (PlayerPrefs.GetInt("levelCoinsCollected") < coinsCollected)
            {
                PlayerPrefs.SetInt("levelCoinsCollected", coinsCollected);
            }
        }
        else
        {
            PlayerPrefs.SetInt("levelCoinsCollected", coinsCollected);
        }
        PlayerPrefs.SetInt("levelCoinsTotal", totalCoins.Length);
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
