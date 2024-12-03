using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class Main_menu : MonoBehaviour
{
    public GameObject HelpGuide, DeleteScreen, MainPanel;
    public AudioSource Audio;
    public AudioClip BackSound;
    public AudioClip ButtonSound;
    
    public Image FadeImage;
    public TextMeshProUGUI highScoreText, coinsCollected, totalCoins;
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
    public void PlayGame()
    {
        PlaySound(ButtonSound);
        StartCoroutine(LoadGame());
        FadeImage.GetComponent<Image>().raycastTarget = true;
    }

    public void QuitGame()
    {
        Application.Quit();
        PlaySound(BackSound);
    }

    public void LoadHelpGuide()
    {
        HelpGuide.SetActive(true);      
        PlaySound(ButtonSound);
    }

    public void CloseHelpGuide()
    {
        HelpGuide.SetActive(false);
        PlaySound(BackSound);
    }

    public void CloseDeleteScreen()
    {
        DeleteScreen.SetActive(false);       
        MainPanel.SetActive(true);
        levelInfo.SetActive(true);
        PlaySound(BackSound);
    }
    public void OpenDeleteScreen()
    {
        DeleteScreen.SetActive(true);
        MainPanel.SetActive(false);
        levelInfo.SetActive(false);
        PlaySound(ButtonSound);
    }
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
