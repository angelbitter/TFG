using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main_menu : MonoBehaviour
{

    public GameObject OptionsMenu;

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void LoadOptionsMenu()
    {
        OptionsMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
