using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// La clase <c>Beat_marker</c> se encarga de gestionar los marcadores de ritmo de los tutoriales sonoros
/// </summary>
/// <remarks>
/// Estos marcadores son los que aparecen por el nivel y que indican al jugador cuándo debe pulsar el botón para realizar habilidades rítmicas
/// </remarks>
public class Beat_marker : MonoBehaviour
{
    /// <summary>
    /// El tamaño al que se expandirá el marcador cuando reciba un evento OnBeat de Beat_manager
    /// </summary>
    [SerializeField] private float PulseSize = 1.15f;
    /// <summary>
    /// La velocidad a la que el marcador volverá a su tamaño original
    /// </summary>
    [SerializeField] private float ReturnSpeed = 5f;
    /// <summary>
    /// Un booleano que indica si baqueta está dentro del rango del marcador
    /// </summary>
    public bool play = false;
    /// <summary>
    /// La escala original del marcador
    /// </summary>
    private Vector3 OriginalScale;
    private int beatCounter;
    /// <summary>
    /// Un array de UnityEvents que se invocarán en función del contador de beats
    /// </summary>
    /// <remarks>
    /// Cada UnityEvent se corresponde con una barra unica del marcador
    /// </remarks>
    public UnityEvent[] beatEvents;

    void Start()
    {
        OriginalScale = transform.localScale;
        beatCounter = 0;
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
    /// Método CountBeatsOnSongMode, se encarga de contar las corcheas segun el metodo OnBeat se invoca desde Beat_manager y llamar a los eventos correspondientes
    /// </summary>
    public void CountBeatsOnSongMode(){
        if(beatCounter == 7)
            beatCounter = 0;
        else
            beatCounter++;
        beatEvents[beatCounter].Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            play = true;
            Baqueta_movement.instance.SetListening(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            play = false;
            Baqueta_movement.instance.SetListening(false);
        }
    }
}
