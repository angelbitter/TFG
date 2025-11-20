using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// La clase <c>SongMarkers_controller</c> se encarga de gestionar los marcadores de los beats de la canción
/// </summary>
public class SongMarkers_controller : MonoBehaviour
{
    public Beat_marker[] markers;
    // Start is called before the first frame update
    public void OnPulse()
    {
        foreach (Beat_marker marker in markers)
        {
            marker.CountBeatsOnSongMode();
        }
    }
}
