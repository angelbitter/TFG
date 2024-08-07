using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause_menu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public AudioSource Audio;
    public AudioSource Music;
    public AudioClip BackSound;
    public AudioClip ButtonSound;
    public AudioClip PauseGameSound;

    public Image FadeImage;
    [SerializeField] GameObject PauseMenu;
    public GameObject HelpGuide;
    
    void Start()
    {
        PauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        GameIsPaused = true;
        PlaySound(PauseGameSound);

        Music.Pause(); 
        PauseMenu.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        PlaySound(ButtonSound);
        
        Music.UnPause();
        PauseMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        PlaySound(BackSound);

        StartCoroutine(LoadMainMenu());
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

    public void PlaySound(AudioClip clip)
    {
        if (Audio != null && clip != null)
        {
            Audio.PlayOneShot(clip);
        }
    }
     private IEnumerator LoadMainMenu()
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
        SceneManager.LoadSceneAsync(0);
    }
}