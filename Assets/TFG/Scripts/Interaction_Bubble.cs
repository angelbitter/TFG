using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Bubble : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer sr;
    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Listening", Baqueta_movement.instance.Listening);
        if (Baqueta_movement.instance.interactionBubbleDirection)
        {
            sr.flipX = false;
        }
        else 
        {
            sr.flipX = true;
        }
    }
}
