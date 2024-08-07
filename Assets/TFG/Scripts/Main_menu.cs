using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main_menu : MonoBehaviour
{

    public GameObject HelpGuide;
    
    public AudioSource Audio;
    public AudioClip BackSound;
    public AudioClip ButtonSound;
    

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
        PlaySound(ButtonSound);
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

    public void PlaySound(AudioClip clip)
    {
        if (Audio != null && clip != null)
        {
            Audio.PlayOneShot(clip);
        }
    }
}
