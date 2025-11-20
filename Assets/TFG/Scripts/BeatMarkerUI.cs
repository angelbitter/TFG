using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// La clase <c>BeatMarkerUI</c> se encarga de gestionar los marcadores de ritmo del HUD del jugador
/// </summary>
/// <remarks>
/// Este marcador es el que aparece en la parte superior de la pantalla y que indica al jugador cuándo debe pulsar el botón para realizar habilidades rítmicas
/// </remarks>
public class BeatMarkerUI : MonoBehaviour
{
    [SerializeField] private float PulseSize = 1.15f;
    [SerializeField] private float ReturnSpeed = 5f;
    
    private Vector3 OriginalScale;
    
    /// <summary>
    /// Array con cada uno de las barras que conforman el marcador
    /// </summary>
    public LineBeatMarker[] lineBeatMarkers;

    void Start()
    {
        OriginalScale = transform.localScale;
    }
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale,OriginalScale, Time.deltaTime * ReturnSpeed);
    }

    /// <summary>
    /// Método Pulse, se encarga de expandir el marcador
    /// </summary> 
    public void Pulse()
    {
        transform.localScale = OriginalScale * PulseSize;
    }
    /// <summary>
    /// Método OnSongMode, se encarga de llamar al método OnSongMode de cada una de las barras cuando se recibe el evento OnSongMode de BeatManager
    /// </summary>
    public void OnSongMode()
    {
        foreach (LineBeatMarker lineBeatMarker in lineBeatMarkers)
        {
            lineBeatMarker.OnSongMode();
        }
    }
    /// <summary>
    /// Método OnSongMode, se encarga de llamar al método OnSongMode de cada una de las barras cuando se recibe el evento OnSongModeEnd de BeatManager
    /// </summary>
    public void OnSongModeEnd()
    {
        foreach (LineBeatMarker lineBeatMarker in lineBeatMarkers)
        {
            lineBeatMarker.OnSongModeEnd();
        }
    }
    /// <summary>
    /// Método OnSongMode, se encarga de llamar al método OnSongMode de cada una de las barras cuando se recibe el evento OnBeat de BeatManager
    /// </summary>OnBeat()
    public void OnBeat()
    {
        foreach (LineBeatMarker lineBeatMarker in lineBeatMarkers)
        {
            lineBeatMarker.ResetPosition();
        }
    }
}
