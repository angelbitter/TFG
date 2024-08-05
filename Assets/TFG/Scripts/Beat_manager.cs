using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Beat_manager : MonoBehaviour
{

    const int INNER_BEATS = 8;
    [SerializeField] private float Bpm;
    [SerializeField] public AudioSource Audio;
    private AudioSource AudioClips;
    public AudioClip OnBeatClip;
    [SerializeField] private Intervals[] IntervalArray;
    [SerializeField] public bool[] SongModeArray;
    public UnityEvent ImpulseEvent;
    public UnityEvent ShootEvent;
    private bool Song = false;
    public int SongModeBeatCounter;
    public bool FailedBeat = false;

    private void Start()
    {
        SongModeArray = new bool[INNER_BEATS];
        AudioClips = GetComponent<AudioSource>();
        SongModeBeatCounter = 0;
    }
    private void Update()
    {
        foreach (Intervals i in IntervalArray)
        {
            float sampledTime = Audio.timeSamples / (Audio.clip.frequency * i.GetIntervalLength(Bpm));
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
     public void CheckSongModeBeat(float sampledTime, int note){
        foreach (Intervals i in IntervalArray)
        {
            if (i.BeatDivision == 2.0f){
                i.CheckOnSongModeBeat(sampledTime, Bpm, note, SongModeBeatCounter);
            }
        }
    }
    public void StartSongMode(){
        Song = true;
        FailedBeat = false;
        SongModeBeatCounter = 0;
    }
    public void EndSongMode(){
        Song = false;
        string songKey = "";

        for (int i = 0; i <  SongModeArray.Length; i++)
        {
            if ( SongModeArray[i])
            {
                songKey += i.ToString();
            }
        }
        if (!FailedBeat){
            switch (songKey)  //remember it starts from 0 to 7
            {
                case "135":
                    ImpulseEvent.Invoke();
                    break; 
                case "235":
                    ShootEvent.Invoke();
                    break;
                default:
                    break;
            }
        }
        SongModeArray = new bool[INNER_BEATS];
        SongModeBeatCounter = 0;
    }
    
    public void CountBeatsOnSongMode(){
        if(Song){
            if(SongModeBeatCounter == INNER_BEATS)
                SongModeBeatCounter = 0;
            else
                SongModeBeatCounter++;
                
            Debug.Log("BeatCounter: " + SongModeBeatCounter);
        }
    }
    public void PlayOnBeatClip(){
            if (Song && OnBeatClip != null)
                AudioClips.PlayOneShot(OnBeatClip, 0.5f);
        }
}

[System.Serializable]
public class Intervals{
    [SerializeField] public float BeatDivision;
    [SerializeField] private UnityEvent OnBeat;
    [SerializeField] private UnityEvent OnWrongBeat;
    [SerializeField] private UnityEvent OnCorrectBeat;
    [SerializeField] private UnityEvent SongModeEndEvent;

    [SerializeField] public Beat_manager BeatManager;
    private bool SongMode = false;
    private bool NewBeat = false;
    public float Threshold = 0.1f;
    public float Threshold2 = 0.05f;
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
            if ((intervalLength - Threshold) < intervalPos)
            {
                NewBeat = false;
            }else{
                BeatManager.SongModeBeatCounter = 1;
            }
        }
        else
        {
            OnWrongBeat.Invoke();
        }
    }
    public void CheckOnSongModeBeat(float time, float bpm, int note, int beatCounter)
    {
        float intervalLength = GetIntervalLength(bpm);
        float intervalPos = time % intervalLength;
        if (note < 3){
            if (Mathf.Abs(intervalPos  - intervalLength) < Threshold2  || Mathf.Abs(intervalPos) < Threshold2)
            { 
                
                OnCorrectBeat.Invoke();
                if ((intervalLength - Threshold2) < intervalPos){
                    Debug.Log("IntervalLenght: " + (Mathf.Abs(intervalPos  - intervalLength)) + " IntervalPos: " + intervalPos + " BeatCounter: " + beatCounter  );
                    BeatManager.SongModeArray[beatCounter - 1] = true;
                }
                else{
                    
                Debug.Log("IntervalLenght: " + (Mathf.Abs(intervalPos  - intervalLength)) + " IntervalPos: " + intervalPos + " BeatCounter: " + beatCounter + "-1");
                    BeatManager.SongModeArray[beatCounter - 2] = true;
                }
            }
            else {
                Debug.Log("IntervalLenght: " + (Mathf.Abs(intervalPos  - intervalLength) ) + " threshold: " + Threshold2 + " BeatCounter: " + beatCounter );
                OnWrongBeat.Invoke();
            }
        }
        else{
            BeatManager.FailedBeat = true;
            OnWrongBeat.Invoke();
        }
    }
}
