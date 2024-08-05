using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class Baqueta_movement : MonoBehaviour
{
    private Rigidbody2D Rb;
    public GameObject SoundWave;
    private float Horizontal;
    private bool IsGrounded;
    private bool WasFlying;
    private bool BeatTriggered;
    private bool FailBeatTriggered;
    private bool ImpulseBool;
    private float originalGravityScale;
    private bool Song;
    private int SongResult = 0;
    
    private int SongModeBeatCounter = 0;

    [SerializeField] private UnityEvent ShowWrongVFX;
    [SerializeField] private UnityEvent ShowRightVFX;

    public float Speed;
    public float MaxSpeed = 1.5f;
    public float Acceleration = 10.0f;
    public float AirAcceleration = 5.0f;
    public float JumpForce = 3.0f;
    public Vector2 ImpulseAngle = new Vector2(0.8f, 0.8f);
    public Transform GroundCheck;
    public LayerMask WhatIsGround;    
    private Vector3 OriginalScale;
    [SerializeField] private float PulseSize = 1.15f;
    [SerializeField] private float ReturnSpeed = 5f;
    
    public AudioSource Audio;
     
    public AudioClip JumpAudio;
    public AudioClip LandAudio;
    public AudioClip OnRightSongAudio;
    public AudioClip OnRightBeatAudio;
    public AudioClip OnWrongSongAudio;
    public AudioClip OnWrongBeatAudio;

    protected Animator Animator;
    public Beat_manager beatManager;

    void Start()
    {
        Audio = GetComponent<AudioSource>();
        Rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        originalGravityScale = Rb.gravityScale;
        ImpulseAngle.Normalize();
        OriginalScale = transform.localScale;
        Jump();
    }

    void Update()
    {
        Animator.SetBool("running", Horizontal != 0.0f);
        Animator.SetBool("air", !IsGrounded);
        Animator.SetBool("fail", FailBeatTriggered);
        Animator.SetBool("songMode", Song);
        Animator.SetBool("impulsed", ImpulseBool);
        
        // para la animación de "pulsar"
        transform.localScale = Vector3.Lerp(transform.localScale,OriginalScale, Time.deltaTime * ReturnSpeed);

        Horizontal = Input.GetAxisRaw("Horizontal");
        
        if (!Song){
            if(Horizontal < 0 && IsGrounded)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                OriginalScale = transform.localScale;
                ImpulseAngle = new Vector2(-0.8f, 0.8f);
            }else if(Horizontal > 0 && IsGrounded)
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                OriginalScale = transform.localScale;
                ImpulseAngle = new Vector2(0.8f, 0.8f);
            }

            IsGrounded = Physics2D.OverlapCircle(GroundCheck.position, 0.1f, WhatIsGround);
            if(IsGrounded && WasFlying)
            {
                PlaySound(LandAudio);
                WasFlying = false;
            }

            if (IsGrounded && !BeatTriggered)
            {
                ImpulseBool = false;
                WasFlying = false;
            }

            if (Input.GetKeyDown(KeyCode.Space)  && IsGrounded)
            {
                Jump();
            }
            if (Input.GetKeyDown(KeyCode.E) && !FailBeatTriggered && !BeatTriggered)
                {
                    //action button - start of SongMode
                    SongModeBeatCounter=0;
                    float sampledTime = (float)beatManager.Audio.timeSamples / beatManager.Audio.clip.frequency;
                    beatManager.CheckSongMode(sampledTime);
                }
            }
            else {
                if (Input.GetKeyDown(KeyCode.E) && Song)
                {
                    //action button - Song Mode Beats
                    float sampledTime = (float)beatManager.Audio.timeSamples / beatManager.Audio.clip.frequency;
                    beatManager.CheckSongModeBeat(sampledTime, SongModeBeatCounter);
                    SongModeBeatCounter++;
                }  
            }
        
    }
    private void Jump()
    {
        Rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        PlaySound(JumpAudio);
    }
    private void FixedUpdate()
    {
        if (ImpulseBool){
            return;
        }
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
                WasFlying = true;
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
        Pulse();
        ShowRightVFX.Invoke();
        PlaySound(OnRightBeatAudio);
        Rb.velocity = Vector2.zero;
        Rb.gravityScale = 0;
        Speed = 0;
        Song = true;
        BeatTriggered = true;
    
     }
     public void OnRightBeatSongMode(){
        ShowRightVFX.Invoke();
        PlaySound(OnRightBeatAudio);
        Pulse();
     }

    public void OnWrongBeat()
    {
        FailBeatTriggered = true;
        PlaySound(OnWrongBeatAudio);
        ShowWrongVFX.Invoke();
        Speed = 0;
        StartCoroutine(ResetFailBeat());
    }
    public void OnWrongBeatSongMode(){
        PlaySound(OnWrongBeatAudio);
        SongResult = 0;
    }

    public void SongModeEnd()
    {
        Song = false;
        SongModeBeatCounter = 0;
        Rb.gravityScale = originalGravityScale;
        if (SongResult == 0)
        {
            ShowWrongVFX.Invoke();
            PlaySound(OnWrongSongAudio);
        }else{
            Pulse();
            PlaySound(OnRightSongAudio);
            switch (SongResult)
            {
                case 1:
                    Impulse();
                    break;
                case 2:
                    ShootSoundWave();
                    break;
                default:
                    break;

        //abierta la puerta a más acciones

            }
        }
        
        SongResult = 0;

        StartCoroutine(ResetBeatTriggered());
    }
    public void CorrectImpulse()
    {
        SongResult = 1;
    }

    public void CorrectSoundWave()
    {
        SongResult = 2;
    }

    public void Impulse()
    {
        ImpulseBool = true;
        Rb.velocity = ImpulseAngle * JumpForce;
    }

    public void ShootSoundWave()
    {
        Rb.velocity = Vector2.zero;
        Rb.gravityScale = 0;
        Speed = 0;
        StartCoroutine(ResetShootSoundwave());
    }

    private IEnumerator ResetFailBeat()
    {
        yield return new WaitForSeconds(0.5f);
        FailBeatTriggered = false;
    }
    private IEnumerator ResetBeatTriggered()
    {
        yield return new WaitForSeconds(0.5f);
        BeatTriggered = false;
    }
    private IEnumerator ResetShootSoundwave()
    {
        yield return new WaitForSeconds(0.25f);
        Vector3 direction;
        if (transform.localScale.x > 0)
        {
            direction = Vector3.right;
        }
        else
        {
            direction = Vector3.left;
        }
        GameObject soundWave = Instantiate(SoundWave, transform.position + direction * 0.2f + new Vector3(0, 0.1f, 0), Quaternion.identity) as GameObject;
        soundWave.GetComponent<Sound_wave_script>().SetDirection(direction);
        
        Rb.gravityScale = originalGravityScale; 
    
    }

    public void Pulse()
    {
        transform.localScale = OriginalScale * PulseSize;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && !IsGrounded)
        {
            Speed = 0;
            if (ImpulseBool)
            {
                Rb.velocity = Vector2.zero;
                ImpulseBool = false; 
            }
        }
    }

    // Baqueta Sounds

    public void PlaySound(AudioClip clip)
    {
        if (Audio != null && clip != null)
        {
            Audio.PlayOneShot(clip);
        }
    }
}
