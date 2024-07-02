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
    public void StartSongMode(){
        foreach (Intervals i in IntervalArray)
        {
            i.Threshold = 0.1f;
        }
    }
}

[System.Serializable]
public class Intervals{
    [SerializeField] public float BeatDivision;
    [SerializeField] private UnityEvent OnBeat;
    [SerializeField] private UnityEvent OnWrongBeat;
    [SerializeField] private UnityEvent OnCorrectBeat;
    [SerializeField] private UnityEvent SongModeEndEvent;
    private bool SongMode = false;
    private bool NewBeat = false;
    public float Threshold = 0.1f;
    const float FULL_MEASURE = 0.25f;

    private int LastInterval  = 0;

    public float GetIntervalLength(float bpm){
        return 60f / (bpm * BeatDivision);
    }

    public void CheckForNewInterval (float interval)
    {
        if(Mathf.FloorToInt(interval) != LastInterval)
        {
            LastInterval = Mathf.FloorToInt(interval);
            if (SongMode && NewBeat){
                Debug.Log(" End of Measure!");
                SongModeEndEvent.Invoke();
                SongMode = false;
            }
            NewBeat = true;
            OnBeat.Invoke();
        }
    }

    public void CheckOnBeat(float time, float bpm)
    {
        float intervalLength = GetIntervalLength(bpm);
        float intervalPos = time % intervalLength;
        if (Mathf.Abs(intervalPos  - intervalLength) < Threshold  || Mathf.Abs(intervalPos) < Threshold)
        {  
            OnCorrectBeat.Invoke();
            SongMode = true;
            Debug.Log("IntervalLenght: " + (intervalLength - Threshold) + " IntervalPos: " + intervalPos);
            if ((intervalLength - Threshold) < intervalPos)
            {
                NewBeat = false;
            }
        }
        else
        {
            OnWrongBeat.Invoke();
        }
    }
}
