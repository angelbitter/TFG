using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
/// <summary>
/// La clase <c>Main_menu</c> se encarga de controlar el menu principal del juego
/// </summary>
public class Main_menu : MonoBehaviour
{
    /// <summary>
    /// Referencias a los paneles de ayuda, borrar datos y el panel principal
    /// </summary>
    public GameObject HelpGuide, DeleteScreen, MainPanel;
    public AudioSource Audio;
    public AudioClip BackSound;
    public AudioClip ButtonSound;

    /// <summary>
    /// La imagen que hace la vez de transición
    /// </summary>    
    public Image FadeImage;
    /// <summary>
    /// El texto que muestra la puntuación más alta y las monedas recogidas
    /// </summary>
    public TextMeshProUGUI highScoreText, coinsCollected, totalCoins;
    /// <summary>
    /// El panel que muestra la información del nivel, como la puntuación y las monedas recogidas
    /// </summary>
    public GameObject levelInfo;


    void Start()
    {
        if(PlayerPrefs.HasKey("levelPoints"))
        {
            highScoreText.text = "Your score: " + PlayerPrefs.GetInt("levelPoints");
            coinsCollected.text = "" + PlayerPrefs.GetInt("levelCoinsCollected");
            totalCoins.text = "/ " + PlayerPrefs.GetInt("levelCoinsTotal");
            levelInfo.SetActive(true);
        }
        else
        {
            levelInfo.SetActive(false);
        }
    }
    /// <summary>
    /// Método que se llama cuando el jugador pulsa el boton de jugar
    /// </summary>
    public void PlayGame()
    {
        PlaySound(ButtonSound);
        StartCoroutine(LoadGame());
        FadeImage.GetComponent<Image>().raycastTarget = true;
    }
    /// <summary>
    /// Método que se llama cuando el jugador pulsa el boton de salir
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
        PlaySound(BackSound);
    }
    /// <summary>
    /// Método que se llama cuando el jugador pulsa el boton de ayuda para carguar la guia de ayuda
    /// </summary>
    public void LoadHelpGuide()
    {
        HelpGuide.SetActive(true);      
        PlaySound(ButtonSound);
    }
    /// <summary>
    /// Método que se llama cuando el jugador pulsa el boton de cerrar la guia de ayuda
    /// </summary>
    public void CloseHelpGuide()
    {
        HelpGuide.SetActive(false);
        PlaySound(BackSound);
    }
    /// <summary>
    /// Método que se llama cuando el jugador pulsa el boton de cerrar la ventana de borrar datos
    /// </summary>
    public void CloseDeleteScreen()
    {
        DeleteScreen.SetActive(false);       
        MainPanel.SetActive(true);
        levelInfo.SetActive(true);
        PlaySound(BackSound);
    }
    /// <summary>
    /// Método que se llama cuando el jugador pulsa el boton de borrar datos en forma de papelera
    /// </summary>
    public void OpenDeleteScreen()
    {
        DeleteScreen.SetActive(true);
        MainPanel.SetActive(false);
        levelInfo.SetActive(false);
        PlaySound(ButtonSound);
    }
    /// <summary>
    /// Método que se llama cuando el jugador confirma que quiere borrar los datos
    /// </summary>
    public void DeleteData()
    {
        PlayerPrefs.DeleteAll();
        DeleteScreen.SetActive(false);
        levelInfo.SetActive(false);
        MainPanel.SetActive(true);
        PlaySound(ButtonSound);
        highScoreText.text = "Your score:" + PlayerPrefs.GetInt("levelPoints");
        coinsCollected.text = "" + PlayerPrefs.GetInt("levelCoinsCollected");
        totalCoins.text = "/ " + PlayerPrefs.GetInt("levelCoinsTotal");
    }
    /// <summary>
    /// Método que se llama para que suene un sonido especifico
    /// </summary>
    /// <param name="clip">Clip de audio que sonará</param>
    public void PlaySound(AudioClip clip)
    {
        if (Audio != null && clip != null)
        {
            Audio.PlayOneShot(clip);
        }
    }

    private IEnumerator LoadGame()
    {
        // Fade out

        float elapsedTime = 0f;
        float fadeDuration = 1f;
        Color startColor = FadeImage.color = new Color(0, 0, 0, 0);
        Color endColor = new Color(0, 0, 0, 1);
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            FadeImage.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }
        FadeImage.color = endColor;
        
        yield return new WaitForSeconds(0.2f);
        FadeImage.GetComponent<Image>().raycastTarget = false; 

        SceneManager.LoadSceneAsync(1);
    }
}
