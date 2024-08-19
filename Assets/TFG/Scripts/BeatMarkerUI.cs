using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BeatMarkerUI : MonoBehaviour
{
    [SerializeField] private float PulseSize = 1.15f;
    [SerializeField] private float ReturnSpeed = 5f;
    
    private Vector3 OriginalScale;
    public LineBeatMarker[] lineBeatMarkers;

    void Start()
    {
        OriginalScale = transform.localScale;
    }
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale,OriginalScale, Time.deltaTime * ReturnSpeed);
    }

    public void Pulse()
    {
        transform.localScale = OriginalScale * PulseSize;
    }
    public void OnSongMode()
    {
        foreach (LineBeatMarker lineBeatMarker in lineBeatMarkers)
        {
            lineBeatMarker.OnSongMode();
        }
    }
    public void OnSongModeEnd()
    {
        foreach (LineBeatMarker lineBeatMarker in lineBeatMarkers)
        {
            lineBeatMarker.OnSongModeEnd();
        }
    }
}
