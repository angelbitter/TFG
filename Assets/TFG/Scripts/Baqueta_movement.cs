using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class Baqueta_movement : MonoBehaviour
{
    private Rigidbody2D Rb;
    private float Horizontal;
    private bool IsGrounded;
    private bool BeatTriggered;
    private bool FailBeatTriggered;
    private float originalGravityScale;
    private bool Song;
    private int SongResult = 0;
    private bool[] SongModeBeats;
    
    private int SongModeBeatCounter;

    [SerializeField] private UnityEvent ShowWrongVFX;
    [SerializeField] private UnityEvent ShowRightVFX;

    public float Speed;
    public float MaxSpeed = 2.0f;
    // public float TimeToMaxSpeed = 0.5f;
    public float Acceleration = 10.0f;
    public float AirAcceleration = 5.0f;
    public float JumpForce = 3.0f;
    public Transform GroundCheck;
    public LayerMask WhatIsGround;    

    protected Animator Animator;
    public Beat_manager beatManager;

    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
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
            if (IsGrounded)
            {
                BeatTriggered = false;
            }

            if (Input.GetKeyDown(KeyCode.Space)  && IsGrounded)
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.E) && !FailBeatTriggered && !Song && !BeatTriggered)
            {
                //action button - start of SongMode
                SongModeBeatCounter++;
                float sampledTime = (float)beatManager.Audio.timeSamples / beatManager.Audio.clip.frequency;
                beatManager.CheckSongMode(sampledTime);
            }

            if (Input.GetKeyDown(KeyCode.E) && !FailBeatTriggered && Song && SongModeBeatCounter < 4)
            {
                //action button - start of SongMode
                float sampledTime = (float)beatManager.Audio.timeSamples / beatManager.Audio.clip.frequency;
                beatManager.CheckSongModeBeat(sampledTime, SongModeBeatCounter);
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
        ShowRightVFX.Invoke();
        Rb.velocity = Vector2.zero;
        Rb.gravityScale = 0;
        Speed = 0;
        Song = true;
        BeatTriggered = true;
    
     }
    public void OnWrongBeat()
    {
        FailBeatTriggered = true;
        ShowWrongVFX.Invoke();
        Speed = 0;
        StartCoroutine(ResetFailBeat());
    }
    public void SongModeEnd()
    {
        Song = false;
        Rb.gravityScale = originalGravityScale;
        switch (SongResult)
        {
            case 0:
                ShowVisualEffect("WrongBeatVFX");
                break;
            case 1:
                // Impulse();
                Jump();
                // ShowVisualEffect("CorrectSongVFX");
                Debug.Log("CorrectImpulse");

                break;
            case 2:
                // ShootSoundWave(); 
                // ShowVisualEffect("CorrectSongVFX");
                Debug.Log("CorrectSoundWave");
                break;
        }

    }
    public void CorrectImpulse()
    {
        SongResult = 1;
    }

    public void CorrectSoundWave()
    {
        SongResult = 2;
    }

    private IEnumerator ResetFailBeat()
    {
        yield return new WaitForSeconds(0.5f);
        FailBeatTriggered = false;
    }
}
