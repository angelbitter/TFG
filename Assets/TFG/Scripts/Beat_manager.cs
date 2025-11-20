using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// La clase <c>Beat_manager</c> se encarga de gestionar los beats y los intervalos de tiempo
/// </summary>
/// <remarks>
/// Lleva la cuenta de cada intervalo de tiempo, los cuales tienen tamaños diferentes y emiten distintos eventos
/// </remarks> 
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
    /// <summary>
    ///  Evento que se dispara al realizar correctamente la habilidad del impulso
    /// </summary>
    public UnityEvent impulseEvent;
    /// <summary>
    /// Evento que se dispara al realizar correctamente la habilidad de disparo
    /// </summary>
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
    /// <summary>
    /// Método CheckSongMode, se encarga de comprobar cuando es llamado si la accion realizada por el jugador está sincronizada con el ritmo de la canción
    /// </summary>
    /// <remarks>
    /// Esto se llama unicamente cuando el jugador entra en el SongMode
    /// </remarks>
    public void CheckSongMode(){
        foreach (Intervals i in intervalArray)
        {
            // Comprobamos si es el inicio del compás 4/4 (que suena 1 de cada 4 pulsos o beats)
            if (i.beatDivision == 0.25f){
                i.CheckOnBeat( bpmMain);
            }
        }
    }
    /// <summary>
    /// Método CheckSongModeBeat, se encarga de comprobar si el jugador ha pulsado el botón en el momento correcto en el SongModeç
    ///  </summary>
    /// <param name="note">El numero de nota que ha pulsado el jugador</param>
    /// <remarks>
    /// Esto se llama unicamente cuando el jugador está ya en el SongMode y toca el ritmo de una cacnión en concreto
    /// </remarks>
     public void CheckSongModeBeat( int note){
        foreach (Intervals i in intervalArray)
        {
            if (i.beatDivision == 2.0f){
                i.CheckOnSongModeBeat( bpmMain, note);
            }
        }
    }
    /// <summary>
    /// Método StartSongMode, se encarga de activar el modo SongMode
    /// </summary>
    public void StartSongMode(){
        song = true;
        failedBeat = false;
    }
    /// <summary>
    /// Método EndSongMode, se encarga de desactivar el modo SongMode y enviar el evento correspondiente al ritmo realizado por el jugador
    /// </summary>
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
    /// <summary>
    /// Método PlayOnBeatClip, se encarga de reproducir un clip de audio cuando el jugador realiza una acción en el momento correcto
    /// </summary>
    public void PlayOnBeatClip(){
            if (song && onBeatClip != null)
            {
                audioClips.PlayOneShot(onBeatClip, 1f);
            }
        }
    /// <summary>
    /// Método PlayMusic, se encarga de parar la música de fondo cuando el jugador está en un menú
    /// </summary> 
    public void StopMusic(){
        audioSong.Stop();
    }
    /// <summary>
    /// Método PlayWinAudio, se encarga de reproducir un clip de audio cuando el jugador gana la partida
    /// </summary>
    public void PlayWinAudio(){
        audioSong.PlayOneShot(winAudio, 2f);
    }
}
/// <summary>
/// La clase <c>Intervals</c> se encarga de gestionar los intervalos de tiempo que se usan para marcar el ritmo
/// </summary>
/// <remarks>
/// Cada intervalo tiene un tamaño y emite eventos distintos
/// </remarks>

[System.Serializable] public class Intervals{

    /// <summary>
    /// El tamaño del intervalo, puede ser 4/4, 2/8, 1/4 o 1/8
    /// </summary>
    [SerializeField] public float beatDivision;
    [SerializeField] private int number;
    [SerializeField] private UnityEvent onBeat;
    [SerializeField] private UnityEvent onWrongBeat;
    [SerializeField] private UnityEvent onCorrectBeat;
    [SerializeField] private UnityEvent songModeEndEvent;
    private bool catchUp = false;
    private bool songMode = false;
    private bool newBeat = false;
    private float threshold = 0.1f;

    /// <summary>
    /// El umbral de error para el modo SongMode
    /// </summary>
    public float threshold2 = 0.065f;
    private int lastInterval  = 0;

    /// <summary>
    ///  Método GetIntervalLength, se encarga de calcular la longitud de un intervalo a raiz del bpm de la canción
    /// </summary>
    /// <param name="bpm">El bpm de la canción de fondo</param>
    /// <returns>Devuelve el intervalo en segundos para el tamaño de intervalo</returns>
    public float GetIntervalLength(float bpm){
        return 60f / (bpm * beatDivision);
    }

    /// <summary>
    /// Método CheckForNewInterval, se encarga de comprobar si ha pasado un nuevo intervalo de tiempo
    /// </summary>
    /// <param name="interval">El intervalo de tiempo actual</param>
    /// <remarks>
    /// Este método se llama automáticamente en el Update de Beat_manager y sirve para marcar el ritmo segú el intervalo cambie
    /// </remarks>
    public void CheckForNewInterval (float interval)
    {
        if(Mathf.FloorToInt(interval) != lastInterval)
        {
            if(!catchUp) {
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
            catchUp = false;
        }
    }

    /// <summary>
    /// Método CheckInterval, se encarga de comprobar si ha pasado un nuevo intervalo de tiempo manualmente cuando el jugador pulsa el botón
    /// </summary> 
    /// <remarks>
    /// Esto fue necesario para el modo SongMode, ya que el jugador puede pulsar el botón entre el intervalo en el que los updates de Beat_manager se llaman y el siguiente intervalo no se haya reconocido aún
    /// </remarks> 
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
            catchUp = true;
            newBeat = true;
            onBeat.Invoke();
        }
    }


    /// <summary>
    /// Método CheckOnBeat, se encarga de comprobar si el jugador ha pulsado el botón en el momento correcto para entrar en el modo SongMode
    /// </summary>
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
    /// <summary>
    /// Método CheckOnSongModeBeat, se encarga de comprobar si el jugador ha pulsado el botón en el momento correcto en el modo SongMode
    /// </summary>  
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
