using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Beat_manager : MonoBehaviour
{

    [SerializeField] private float Bpm;
    [SerializeField] public AudioSource Audio;
    [SerializeField] private Intervals[] IntervalArray;

    private void Update()
    {
        foreach (Intervals i in IntervalArray)
        {
            float sampledTime = (Audio.timeSamples / (Audio.clip.frequency * i.GetIntervalLength(Bpm)));
            i.CheckForNewInterval(sampledTime);
        }
    }
    public void CheckSongMode(float sampledTime){
        foreach (Intervals i in IntervalArray)
        {
            // Comprobamos si es el inicio del compás 4/4 (que suena 1 de cada 4 pulsos o beats)
            if (i.BeatDivision == 0.25f){
                i.CheckOnBeat(sampledTime, Bpm);
            }
        }
    
    }
}

[System.Serializable]
public class Intervals{
    [SerializeField] public float BeatDivision;
    [SerializeField] private UnityEvent OnBeat;
    [SerializeField] private UnityEvent OnWrongBeat;
    [SerializeField] private UnityEvent OnCorrectBeat;
    public float Threshold = 0.1f;

    private int LastInterval  = 0;

    public float GetIntervalLength(float bpm){
        return 60f / (bpm * BeatDivision);
    }

    public void CheckForNewInterval (float interval)
    {
        if(Mathf.FloorToInt(interval) != LastInterval)
        {
            LastInterval = Mathf.FloorToInt(interval);
            OnBeat.Invoke();
        }
    }

    public void CheckOnBeat(float time, float bpm)
    {
        float intervalLength = GetIntervalLength(bpm);
        float intervalPos = time % intervalLength;
        // Debug.Log("Intervalo: " + intervalPos);
        if (Mathf.Abs(intervalPos - intervalLength) < Threshold  || Mathf.Abs(intervalPos) < Threshold)
        {
            OnCorrectBeat.Invoke();
        }
        else
        {
            OnWrongBeat.Invoke();
        }
    }
}
