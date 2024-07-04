using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Baqueta_movement : MonoBehaviour
{
    private Rigidbody2D Rb;
    private float Horizontal;
    private float Vertical;
    private bool IsGrounded;
    private bool BeatTriggered;
    private bool FailBeatTriggered;
    private float originalGravityScale;
    private bool Song;

    public float Speed;
    private bool ChangingDirection;
    public float MaxSpeed = 2.0f;
    // public float TimeToMaxSpeed = 0.5f;
    public float Acceleration = 4.0f;
    public float AirAcceleration = 2.0f;
    public float JumpForce = 4.0f;
    public Transform GroundCheck;
    public LayerMask WhatIsGround;    

    protected Animator Animator;
    public Beat_manager beatManager;
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();;
        Animator = GetComponent<Animator>();
        originalGravityScale = Rb.gravityScale;

    }

    void Update()
    {
        Animator.SetBool("running", Horizontal != 0.0f);
        Animator.SetBool("air", !IsGrounded);
        Animator.SetBool("fail", FailBeatTriggered);
        Animator.SetBool("songMode", Song);

        Horizontal = Input.GetAxisRaw("Horizontal");
        
        if (!Song){
            if(Horizontal < 0 && IsGrounded)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }else if(Horizontal > 0 && IsGrounded)
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }

            IsGrounded = Physics2D.OverlapCircle(GroundCheck.position, 0.1f, WhatIsGround);

            if (Input.GetKeyDown(KeyCode.Space)  && IsGrounded)
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.E) && !FailBeatTriggered)
            {
                //action button - start of SongMode
                float sampledTime = (float)beatManager.Audio.timeSamples / beatManager.Audio.clip.frequency;
                beatManager.CheckSongMode(sampledTime);
            }
        }
    }
    private void Jump()
    {
        Rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
    }
    private void FixedUpdate()
    {
        if (!Song){
            if (IsGrounded) {
                if (Horizontal != 0.0f){
                    ChangingDirection = Horizontal * Speed < 0.0f;
                    if (Horizontal > 0 )
                        Speed = Mathf.Min(Speed + Acceleration * Time.deltaTime, MaxSpeed);
                    else
                        Speed = Mathf.Max(Speed - Acceleration * Time.deltaTime, -MaxSpeed);
                }else{
                    Speed = Mathf.MoveTowards(Speed, 0, Acceleration * Time.deltaTime);
                }
            }else{
                //air movement
                if (Horizontal != 0.0f){
                    if (Horizontal > 0 )
                        Speed = Mathf.Min(Speed + AirAcceleration * Time.deltaTime, MaxSpeed);
                    else
                        Speed = Mathf.Max(Speed - AirAcceleration * Time.deltaTime, -MaxSpeed);
                }else{
                    Speed = Mathf.MoveTowards(Speed, 0, AirAcceleration * Time.deltaTime);
                }
            }
            Rb.velocity = new Vector2(Speed , Rb.velocity.y);
        }
    }

    public void OnRightBeat()
    {
        Debug.Log("Start Song Mode");
        Rb.velocity = Vector2.zero;
        Rb.gravityScale = 0;
        Speed = 0;
        Song = true;
     }
    public void OnWrongBeat()
    {
        FailBeatTriggered = true;
        Speed = 0;
        StartCoroutine(ResetFailBeat());
        Debug.Log("wrong Beat");
    }
    public void SongModeEnd()
    {
        Song = false;
        Debug.Log("End Song Mode");
        Rb.gravityScale = originalGravityScale;
    }
    private IEnumerator ResetFailBeat()
    {
        yield return new WaitForSeconds(0.5f);
        FailBeatTriggered = false;
    }
}
