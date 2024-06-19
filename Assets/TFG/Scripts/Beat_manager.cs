using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Beat_manager : MonoBehaviour
{

    [SerializeField] private float Bpm;
    [SerializeField] private AudioSource Audio;
    [SerializeField] private Intervals[] IntervalArray;

    private void Update()
    {
        foreach (Intervals i in IntervalArray)
        {
            float sampledTime = (Audio.timeSamples / (Audio.clip.frequency * i.GetIntervalLength(Bpm)));
            i.CheckForNewInterval(sampledTime);
        }
    }
}

[System.Serializable]
public class Intervals{
    [SerializeField] private float Beats;
    [SerializeField] private UnityEvent OnBeat;
    private int LastInterval  = 0;

    public float GetIntervalLength(float bpm){
        return 60f / (bpm * Beats);
    }

    public void CheckForNewInterval (float interval)
    {
        if(Mathf.FloorToInt(interval) != LastInterval)
        {
            LastInterval = Mathf.FloorToInt(interval);
            OnBeat.Invoke();
        }
    }
}
