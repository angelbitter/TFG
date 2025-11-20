using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// La clase <c>Pause_menu</c> se encarga de gestionar el menú de pausa del juego
/// </summary>
public class Pause_menu : MonoBehaviour
{
    /// <summary>
    /// Variable que indica si el juego esta pausado
    /// </summary>
    public static bool GameIsPaused = false;
    /// <summary>
    /// Variable que indica si se esta cargando una escena, se usa para controlar las transiciones
    /// </summary>
    private bool Loading = false;
    private bool Help = false;

    public AudioSource Audio;
    public AudioSource Music;
    public AudioClip BackSound;
    public AudioClip ButtonSound;
    public AudioClip PauseGameSound;

    public GameObject Baqueta;
    /// <summary>
    /// La imagen que hace la vez de transición con un fundido negro
    /// </summary>
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
    /// <summary>
    /// Método Pause, se llama cuando el jugador pulsa el botoón ESC
    /// </summary>
    public void Pause()
    {
        Time.timeScale = 0f;
        GameIsPaused = true;
        PlaySound(PauseGameSound);

        Music.Pause(); 
        Baqueta.GetComponent<Baqueta_movement>().enabled = false;
        PauseMenu.SetActive(true);
    }
    /// <summary>
    /// Método Resume, se llama cuando el jugador pulsa el boton de continuar
    /// </summary>
    public void Resume()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        PlaySound(ButtonSound);
        
        Music.UnPause();
        
        Baqueta.GetComponent<Baqueta_movement>().enabled = true;
        PauseMenu.SetActive(false);
    }
    /// <summary>
    /// Método que se llama cuando el jugador quiere salir del juego
    /// </summary>
    public void QuitGame()
    {
        Baqueta_movement.instance.DisableBaqueta();
        Time.timeScale = 1f;
        Loading = true;
        PlaySound(BackSound);
        Beat_manager.instance.StopMusic();
        StartCoroutine(LoadScreen(0));
    }
/// <summary>
/// Método que se llama cuando el jugador quiere cargar la guía de ayuda
/// </summary>
public void LoadHelpGuide()
    {
        Help = true;
        HelpGuide.SetActive(true);        
        PlaySound(ButtonSound);
    }
    /// <summary>
    /// Método que se llama cuando el jugador quiere cerrar la guía de ayuda
    /// </summary> 
    public void CloseHelpGuide()
    {
        Help = false;
        HelpGuide.SetActive(false);
        PlaySound(BackSound);
    }
    /// <summary>
    /// Método que se llama cuando el jugador quiere reiniciar el nivel
    /// </summary>
    public void Restart()
    {
        Time.timeScale = 1f;
        PlaySound(ButtonSound);
        StartCoroutine(LoadScreen(1));
        FadeImage.GetComponent<Image>().raycastTarget = true; 
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