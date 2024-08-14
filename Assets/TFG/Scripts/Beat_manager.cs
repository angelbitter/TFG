using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Beat_manager : MonoBehaviour
{

    const int INNER_BEATS = 8;
    [SerializeField] private float bpmMain;
    [SerializeField] public AudioSource audioSong;
    private AudioSource audioClips;
    public AudioClip onBeatClip;
    [SerializeField] private Intervals[] intervalArray;
    [SerializeField] public bool[] songModeArray;
    public UnityEvent impulseEvent;
    public UnityEvent shootEvent;
    private bool song = false;
    public int songModeBeatCounter;
    public bool failedBeat = false;

    private void Start()
    {
        songModeArray = new bool[INNER_BEATS];
        audioClips = GetComponent<AudioSource>();
        songModeBeatCounter = 0;
    }
    private void Update()
    {
        foreach (Intervals i in intervalArray)
        {
            float sampledTime = audioSong.timeSamples / (audioSong.clip.frequency * i.GetIntervalLength(bpmMain));
            i.CheckForNewInterval(sampledTime);
        }
    }
    public void CheckSongMode(float sampledTime){
        foreach (Intervals i in intervalArray)
        {
            // Comprobamos si es el inicio del compás 4/4 (que suena 1 de cada 4 pulsos o beats)
            if (i.beatDivision == 0.25f){
                i.CheckOnBeat(sampledTime, bpmMain);
            }
        }
    }
     public void CheckSongModeBeat(float sampledTime, int note){
        foreach (Intervals i in intervalArray)
        {
            if (i.beatDivision == 2.0f){
                i.CheckOnSongModeBeat(sampledTime, bpmMain, note, songModeBeatCounter);
            }
        }
    }
    public void StartSongMode(){
        song = true;
        failedBeat = false;
        songModeBeatCounter = 0;
    }
    public void EndSongMode(){
        song = false;
        string songKey = "";

        for (int i = 0; i <  songModeArray.Length; i++)
        {
            if ( songModeArray[i])
            {
                songKey += i.ToString();
            }
        }
        if (!failedBeat){
            switch (songKey)  //remember it starts from 0 to 7
            {
                case "135":
                    impulseEvent.Invoke();
                    break; 
                case "235":
                    shootEvent.Invoke();
                    break;
                default:
                    break;
            }
        }
        songModeArray = new bool[INNER_BEATS];
        songModeBeatCounter = 0;
        
    }
    
    public void CountBeatsOnSongMode(){
        if(song){
            if(songModeBeatCounter == INNER_BEATS)
                songModeBeatCounter = 0;
            else
                songModeBeatCounter++;
        }
    }
    public void PlayOnBeatClip(){
            if (song && onBeatClip != null)
                audioClips.PlayOneShot(onBeatClip, 1f);
        }
}

[System.Serializable] public class Intervals{
    [SerializeField] public float beatDivision;
    [SerializeField] private UnityEvent onBeat;
    [SerializeField] private UnityEvent onWrongBeat;
    [SerializeField] private UnityEvent onCorrectBeat;
    [SerializeField] private UnityEvent songModeEndEvent;

    [SerializeField] public Beat_manager beatManager;
    private bool songMode = false;
    private bool newBeat = false;
    private float threshold = 0.1f;
    private float threshold2 = 0.075f;
    private int lastInterval  = 0;

    public float GetIntervalLength(float bpm){
        return 60f / (bpm * beatDivision);
    }

    public void CheckForNewInterval (float interval)
    {
        if(Mathf.FloorToInt(interval) != lastInterval)
        {
            lastInterval = Mathf.FloorToInt(interval);
            if (songMode && newBeat){
                songModeEndEvent.Invoke();
                songMode = false;
            }
            newBeat = true;
            onBeat.Invoke();
        }
    }


    public void CheckOnBeat(float time, float bpm)
    {
        float intervalLength = GetIntervalLength(bpm);
        float intervalPos = time % intervalLength;
        if (Mathf.Abs(intervalPos  - intervalLength) < threshold  || Mathf.Abs(intervalPos) < threshold)
        {  
            onCorrectBeat.Invoke();
            songMode = true;
            if ((intervalLength - threshold) < intervalPos)
            {
                newBeat = false;
            }else{
                beatManager.songModeBeatCounter = 1;
            }
        }
        else
        {
            onWrongBeat.Invoke();
        }
    }
    public void CheckOnSongModeBeat(float time, float bpm, int note, int beatCounter)
    {
        float intervalLength = GetIntervalLength(bpm);
        float intervalPos = time % intervalLength;
        if (note < 3){
            if (Mathf.Abs(intervalPos  - intervalLength) < threshold2  || Mathf.Abs(intervalPos) < threshold2)
            { 
                
                onCorrectBeat.Invoke();
                if ((intervalLength - threshold2) < intervalPos){
                    beatManager.songModeArray[beatCounter - 1] = true;
                }
                else{
                    beatManager.songModeArray[beatCounter - 2] = true;
                }
            }
            else {
                onWrongBeat.Invoke();
            }
        }
        else{
            beatManager.failedBeat = true;
            onWrongBeat.Invoke();
        }
    }
}
