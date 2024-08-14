using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause_menu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    private bool Loading = false;
    private bool Help = false;

    public AudioSource Audio;
    public AudioSource Music;
    public AudioClip BackSound;
    public AudioClip ButtonSound;
    public AudioClip PauseGameSound;

    public GameObject Baqueta;
    public Image FadeImage;
    [SerializeField] GameObject PauseMenu;
    public GameObject HelpGuide;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !Loading)
        {
            if (GameIsPaused)
            {
                if (Help)
                {
                    CloseHelpGuide();
                }
                else
                {
                    Resume();
                }
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
        Baqueta.GetComponent<Baqueta_movement>().enabled = false;
        PauseMenu.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        PlaySound(ButtonSound);
        
        Music.UnPause();
        
        Baqueta.GetComponent<Baqueta_movement>().enabled = true;
        PauseMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Loading = true;
        PlaySound(BackSound);
        StartCoroutine(LoadScreen(0));
    }

public void LoadHelpGuide()
    {
        Help = true;
        HelpGuide.SetActive(true);        
        PlaySound(ButtonSound);
    }
    
    public void CloseHelpGuide()
    {
        Help = false;
        HelpGuide.SetActive(false);
        PlaySound(BackSound);
    }
    public void Restart()
    {
        PlaySound(ButtonSound);
        StartCoroutine(LoadScreen(1));
        FadeImage.GetComponent<Image>().raycastTarget = true; 
    }

    public void PlaySound(AudioClip clip)
    {
        if (Audio != null && clip != null)
        {
            Audio.PlayOneShot(clip);
        }
    }
     private IEnumerator LoadScreen(int index)
    {
        // Fade out

        float elapsedTime = 0f;
        float fadeDuration = 1f;
        Color startColor = FadeImage.color;
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
        Loading = false;
        GameIsPaused = false;
        SceneManager.LoadSceneAsync(index);
    }
}