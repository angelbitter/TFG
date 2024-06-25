using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Baqueta_movement : MonoBehaviour
{
    private Rigidbody2D Rb;
    private float Horizontal;
    private float Vertical;
    private bool IsGrounded;


    public float Speed;
    public float JumpForce;
    public Transform GroundCheck;
    public LayerMask WhatIsGround;    

    protected Animator Animator;
    public Beat_manager beatManager;

    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();;
        Animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");

        Animator.SetBool("running", Horizontal != 0.0f);
        Animator.SetBool("air", !IsGrounded);

        if(Horizontal < 0)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }else if(Horizontal > 0)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        IsGrounded = Physics2D.OverlapCircle(GroundCheck.position, 0.1f, WhatIsGround);

        if (Input.GetKeyDown(KeyCode.Space)  && IsGrounded)
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            float sampledTime = (float)beatManager.Audio.timeSamples / beatManager.Audio.clip.frequency;
            beatManager.CheckSongMode(sampledTime);
        }
    }
    private void Jump()
    {
        Rb.AddForce(Vector2.up * JumpForce);
    }
    private void FixedUpdate()
    {
        Rb.velocity = new Vector2(Horizontal * Speed, Rb.velocity.y);
    }

    public void OnRightBeat()
    {
            Debug.Log("Correct Beat");
    }
     public void OnWrongBeat()
    {
            Debug.Log("wrong Beat");
    }
}
