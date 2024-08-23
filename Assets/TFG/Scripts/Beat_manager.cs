using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Beat_manager : MonoBehaviour
{
    public static Beat_manager instance;
    const int INNER_BEATS = 8;
    [SerializeField] public float bpmMain;
    [SerializeField] public AudioSource audioSong;
    private AudioSource audioClips;
    public AudioClip onBeatClip; public AudioClip winAudio;
    [SerializeField] private Intervals[] intervalArray;
    [SerializeField] public bool[] songModeArray;
    public UnityEvent impulseEvent;
    public UnityEvent shootEvent;
    private bool song = false;
    public bool failedBeat = false;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        songModeArray = new bool[INNER_BEATS];
        audioClips = GetComponent<AudioSource>();
    }
    private void Update()
    {
        foreach (Intervals i in intervalArray)
        {

            float sampledTime = audioSong.timeSamples / (audioSong.clip.frequency * i.GetIntervalLength(bpmMain));
            i.CheckForNewInterval(sampledTime);
        }
    }
    public void CheckSongMode(){
        foreach (Intervals i in intervalArray)
        {
            // Comprobamos si es el inicio del compás 4/4 (que suena 1 de cada 4 pulsos o beats)
            if (i.beatDivision == 0.25f){
                i.CheckOnBeat( bpmMain);
            }
        }
    }
     public void CheckSongModeBeat( int note){
        foreach (Intervals i in intervalArray)
        {
            if (i.beatDivision == 2.0f){
                i.CheckOnSongModeBeat( bpmMain, note);
            }
        }
    }
    public void StartSongMode(){
        song = true;
        failedBeat = false;
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
    }
    
    public void PlayOnBeatClip(){
            if (song && onBeatClip != null)
            {
                audioClips.PlayOneShot(onBeatClip, 1f);
            }
        }
    public void StopMusic(){
        audioSong.Stop();
    }
    public void PlayWinAudio(){
        audioSong.PlayOneShot(winAudio, 2f);
    }
}
[System.Serializable] public class Intervals{
    [SerializeField] public float beatDivision;
    [SerializeField] private int number;
    [SerializeField] private UnityEvent onBeat;
    [SerializeField] private UnityEvent onWrongBeat;
    [SerializeField] private UnityEvent onCorrectBeat;
    [SerializeField] private UnityEvent songModeEndEvent;
    private bool songMode = false;
    private bool newBeat = false;
    private float threshold = 0.1f;
    public float threshold2 = 0.065f;
    private int lastInterval  = 0;

    public float GetIntervalLength(float bpm){
        return 60f / (bpm * beatDivision);
    }

    public void CheckForNewInterval (float interval)
    {
        if(Mathf.FloorToInt(interval) != lastInterval)
        {
            lastInterval = Mathf.FloorToInt(interval);
                if(beatDivision == 2.0f){
                    number++;
                    if (number % 8 == 0){
                        number = 0;
                    }
                }
                if (songMode && newBeat && beatDivision == 0.25f){
                    songModeEndEvent.Invoke();
                    songMode = false;
                }
                newBeat = true;
            onBeat.Invoke();
        }
    }

    //This one is intended to be called manually when the player presses the drum button
    //Since the intervals are not exactly whole numbers, there is some margin of error and
    //checking manually if we are on a different interval is necessary 
    public void CheckInterval (float interval)
    {
        if(Mathf.FloorToInt(interval) != lastInterval)
        {
            lastInterval = Mathf.FloorToInt(interval);
            if(beatDivision == 2.0f){
                number++;
                if (number % 8 == 0){
                    number = 0;
                }
            }
            newBeat = true;
        }
    }


    public void CheckOnBeat( float bpm)
    {
            float time = (float)Beat_manager.instance.audioSong.timeSamples / Beat_manager.instance.audioSong.clip.frequency;
            float sampledTime = Beat_manager.instance.audioSong.timeSamples /(Beat_manager.instance.audioSong.clip.frequency * GetIntervalLength(Beat_manager.instance.bpmMain));
            CheckInterval(sampledTime);
            float intervalLength = GetIntervalLength(bpm);
            float intervalPos = time % intervalLength;
            if (Mathf.Abs(intervalPos  - intervalLength) < threshold  || Mathf.Abs(intervalPos) < threshold)
            {  
                onCorrectBeat.Invoke();
                songMode = true;
                if ((intervalLength - intervalPos) < intervalLength - threshold )
                {
                    newBeat = false;
                }
            }
            else
            {
                onWrongBeat.Invoke();
            }
        
    }
    public void CheckOnSongModeBeat(float bpm, int note)
    {
            float time = (float)Beat_manager.instance.audioSong.timeSamples / Beat_manager.instance.audioSong.clip.frequency;
            float sampledTime = Beat_manager.instance.audioSong.timeSamples /(Beat_manager.instance.audioSong.clip.frequency * GetIntervalLength(Beat_manager.instance.bpmMain));
            CheckInterval(sampledTime);
            float intervalLength = GetIntervalLength(bpm);
            float intervalPos = time % intervalLength;
            int beatNumber = number;
            if (note < 3){
                if (Mathf.Abs(intervalPos  - intervalLength) <  threshold2  || Mathf.Abs(intervalPos) < threshold2)
                { 
                    onCorrectBeat.Invoke();
                    if ((intervalLength - intervalPos) < intervalLength - threshold ){
                        Beat_manager.instance.songModeArray[ beatNumber ] = true;
                    }
                    else{
                    Beat_manager.instance.songModeArray[ beatNumber -1] = true;
                    }
                }
                else {
                    onWrongBeat.Invoke();
                }
            }
            else{
                Beat_manager.instance.failedBeat = true;
                onWrongBeat.Invoke();
            }
    }
}
