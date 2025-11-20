using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// La clase <c>SingleMarker</c> se encarga de gestionar las barras de los marcadores sonoros
/// </summary>
public class SingleMarker : MonoBehaviour
{
    private float PulseSize = 1.15f;
    private float ReturnSpeed = 5f;
    private Vector3 OriginalScale;
    /// <summary>
    /// Booleano que indica si la barra es el primer o el último del marcador
    /// </summary>
    public bool startOrFinish;
    [SerializeField] private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        OriginalScale = transform.localScale;
    }
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale,OriginalScale, Time.deltaTime * ReturnSpeed);
    }
    /// <summary>
    /// Método que se llama cando recibe el evento OnBeat
    /// </summary> 
    /// <remarks>
    /// Dependiendo de si es la primera/última barra o una barra interior, suena un sonido u otro de Baqueta
    public void Pulse()
    {
        transform.localScale = OriginalScale * PulseSize;
        animator.Play("Pulse");
        if(gameObject.GetComponentInParent<Beat_marker>().play){
            if (startOrFinish)
            {
                Baqueta_movement.instance.PlaySongModeBeat2();
            }
            else
            {
                Baqueta_movement.instance.PlaySongModeBeat();
            }
            }
    }
}
