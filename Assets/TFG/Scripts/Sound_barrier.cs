using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// La clase <c>Sound_barrier</c> se encarga de gestionar la barrera de sonido que Baqueta puede destruir
/// </summary>
public class Sound_barrier : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;
    public BoxCollider2D collider2DBarrier;
    public CapsuleCollider2D collider2DBarrierCircle;

    void Start()
    {
        animator = GetComponent<Animator>();
        collider2DBarrier = GetComponent<BoxCollider2D>();     
        collider2DBarrierCircle = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }
    /// <summary>
    /// Método que se llama cuando la barrera de sonido ws atravesada
    /// </summary>
    public void TakeDamage()
    {
        animator.Play("BarrierDestroy");
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        audioSource.Play();
    }
    /// <summary>
    /// Método que se llama cuando la animación de destrucción de la barrera de sonido ha terminado
    /// </summary>
    public void DestroyBarrier()
    {
        Destroy(gameObject);
    }
    /// <summary>
    /// Método que se llama para desactivar la colisión de la barrera de sonido cuando baqueta la atraviesa con el Impulso
    /// </summary>
    public void DeactivateCollision()
    {
        collider2DBarrier.enabled = false;
    }
}
