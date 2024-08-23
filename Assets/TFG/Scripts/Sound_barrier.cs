using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void TakeDamage()
    {
        animator.Play("BarrierDestroy");
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        audioSource.Play();
    }
    public void DestroyBarrier()
    {
        Destroy(gameObject);
    }
    public void DeactivateCollision()
    {
        collider2DBarrier.enabled = false;
    }
}
