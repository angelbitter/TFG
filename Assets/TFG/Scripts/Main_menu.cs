using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_menu : MonoBehaviour
{
   public void PlayGame()
   {
       SceneManager.LoadSceneAsync(1);
   }

    public void LoadOptionsMenu()
    {

    }

    public void QuitGame()
    {
         Application.Quit();
    }
}
