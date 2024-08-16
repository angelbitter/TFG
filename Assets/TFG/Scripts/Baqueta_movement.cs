using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using Unity.VisualScripting.Dependencies.Sqlite;
using Cinemachine;
using Unity.VisualScripting;

public class Baqueta_movement : MonoBehaviour
{
    public static Baqueta_movement instance;
    private Rigidbody2D rb;
    public GameObject soundWave;
    private Collider2D baquetaCollider;
    private float horizontal;
    private bool isGrounded;
    private bool wasFlying;
    private bool beatTriggered;
    private bool failBeatTriggered;
    private bool impulseBool;
    private float originalGravityScale;
    private bool song;
    private int songResult = 0;
    
    private int songModeBeatCounter = 0;

    [SerializeField] private UnityEvent showWrongVFX;
    [SerializeField] private UnityEvent showRightVFX;

    public float speed;
    public float maxSpeed = 1.5f;
    public float acceleration = 10.0f;
    public float airAcceleration = 5.0f;
    public float jumpForce = 3.0f;
    public Vector2 impulseAngle = new Vector2(0.8f, 0.8f);
    public Transform groundCheck;
    public LayerMask whatIsGround;    
    private Vector3 originalScale;
    [SerializeField] private float pulseSize = 1.15f;
    [SerializeField] private float returnSpeed = 5f;
    
    public AudioSource audioSource;     public AudioSource loopingSource;
    public AudioClip landAudio;         public AudioClip onRightSongAudio;
    public AudioClip onRightBeatAudio;  public AudioClip onWrongSongAudio;
    public AudioClip onWrongBeatAudio;  public AudioClip getHitSound;
    public AudioClip impulseAudio;      public AudioClip jumpAudio;
    public AudioClip soundWaveAudio;    public AudioClip soundWaveEndAudio;
    public AudioClip getCoinSound;      public AudioClip getHealthSound;

    protected Animator animator;
    public Beat_manager beatManager;

    private bool vulnerable = true;
    public float vulnerableTime = 1.5f;
    public bool isDead = false;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        baquetaCollider = GetComponent<Collider2D>();
        originalGravityScale = rb.gravityScale;
        impulseAngle.Normalize();
        originalScale = transform.localScale;
        isDead = false;
    }

    void Update()
    {
        animator.SetBool("running", horizontal != 0.0f);
        animator.SetBool("air", !isGrounded);
        animator.SetBool("fail", failBeatTriggered);
        animator.SetBool("songMode", song);
        animator.SetBool("impulsed", impulseBool);

        // para la animación de "pulsar"
        transform.localScale = Vector3.Lerp(transform.localScale,originalScale, Time.deltaTime * returnSpeed);

        if (!isDead) horizontal = Input.GetAxisRaw("Horizontal");
        
        if (!song){
            if(horizontal < 0 && isGrounded)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                originalScale = transform.localScale;
                impulseAngle = new Vector2(-0.8f, 0.8f);
            }else if(horizontal > 0 && isGrounded)
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                originalScale = transform.localScale;
                impulseAngle = new Vector2(0.8f, 0.8f);
            }

            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.06f, whatIsGround);
            if(isGrounded && wasFlying)
            {
                PlaySound(landAudio);
                wasFlying = false;
            }

            if (isGrounded && !beatTriggered)
            {
                impulseBool = false;
                wasFlying = false;
            }

            if (Input.GetButtonDown("Jump")  && isGrounded)
            {
                Jump();
            }
            if (Input.GetButtonDown("Fire1") && !failBeatTriggered && !beatTriggered)
                {
                    //action button - start of SongMode
                    songModeBeatCounter=0;
                    float sampledTime = (float)beatManager.GetComponent<AudioSource>().timeSamples / beatManager.GetComponent<AudioSource>().clip.frequency;
                    beatManager.CheckSongMode(sampledTime);
                }
            }
            else {
                if (Input.GetButtonDown("Fire1") && song)
                {
                    //action button - song Mode Beats
                    float sampledTime = (float)beatManager.GetComponent<AudioSource>().timeSamples / beatManager.GetComponent<AudioSource>().clip.frequency;
                    beatManager.CheckSongModeBeat(sampledTime, songModeBeatCounter);
                    songModeBeatCounter++;
                }  
            }
        
    }
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        PlaySound(jumpAudio);
    }
    private void FixedUpdate()
    {
        if (impulseBool){
            return;
        }
        if (!song){
            if (isGrounded) {
                if (horizontal != 0.0f){
                    if (horizontal > 0 )
                        speed = Mathf.Min(speed + acceleration * Time.deltaTime, maxSpeed);
                    else
                        speed = Mathf.Max(speed - acceleration * Time.deltaTime, -maxSpeed);
                }else{
                    speed = Mathf.MoveTowards(speed, 0, acceleration * Time.deltaTime);
                }
            }else{
                wasFlying = true;
                //air movement
                if (horizontal != 0.0f){
                    if (horizontal > 0 )
                        speed = Mathf.Min(speed + airAcceleration * Time.deltaTime, maxSpeed);
                    else
                        speed = Mathf.Max(speed - airAcceleration * Time.deltaTime, -maxSpeed);
                }else{
                    speed = Mathf.MoveTowards(speed, 0, airAcceleration * Time.deltaTime);
                }
            }
            rb.velocity = new Vector2(speed , rb.velocity.y);
        }
    }

    public void OnRightBeat()
    {   
        Pulse();
        showRightVFX.Invoke();
        PlaySound(onRightBeatAudio);
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        speed = 0;
        song = true;
        beatTriggered = true;
    
     }
     public void OnRightBeatSongMode(){
        showRightVFX.Invoke();
        PlaySound(onRightBeatAudio);
        Pulse();
     }

    public void OnWrongBeat()
    {
        failBeatTriggered = true;
        PlaySound(onWrongBeatAudio);
        showWrongVFX.Invoke();
        speed = 0;
        StartCoroutine(ResetFailBeat());
    }
    public void OnWrongBeatSongMode(){
        PlaySound(onWrongBeatAudio);
        songResult = 0;
    }

    public void SongModeEnd()
    {
        song = false;
        songModeBeatCounter = 0;
        rb.gravityScale = originalGravityScale;
        if (songResult == 0)
        {
            showWrongVFX.Invoke();
            PlaySound(onWrongSongAudio);
        }else{
            Pulse();
            PlaySound(onRightSongAudio);
            switch (songResult)
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
        
        songResult = 0;

        StartCoroutine(ResetBeatTriggered());
    }
    public void CorrectImpulse()
    {
        songResult = 1;
    }

    public void CorrectSoundWave()
    {
        songResult = 2;
    }

    public void Impulse()
    {
        impulseBool = true;
        rb.velocity = impulseAngle * jumpForce;
        loopingSource.PlayOneShot(impulseAudio);
    }

    public void ShootSoundWave()
    {
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        speed = 0;
        loopingSource.PlayOneShot(soundWaveAudio);
        StartCoroutine(ResetShootSoundwave());
    }

    public void OnSoundWaveDestruction(){
        loopingSource.Stop();
        audioSource.PlayOneShot(soundWaveEndAudio);
    }

    private IEnumerator ResetFailBeat()
    {
        yield return new WaitForSeconds(0.5f);
        failBeatTriggered = false;
    }
    private IEnumerator ResetBeatTriggered()
    {
        yield return new WaitForSeconds(0.5f);
        beatTriggered = false;
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
        GameObject soundWaveInst = Instantiate(soundWave, transform.position + direction * 0.2f + new Vector3(0, 0.1f, 0), Quaternion.identity) as GameObject;
        soundWaveInst.GetComponent<Sound_wave_script>().SetDirection(direction);
        
        rb.gravityScale = originalGravityScale; 
    
    }

    public void Pulse()
    {
        transform.localScale = originalScale * pulseSize;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard") && impulseBool)
        {
            impulseBool = true;
            return;
        }
        if (collision.gameObject.CompareTag("Wall") && !isGrounded)
        {
            speed = 0;
            if (impulseBool)
            {
                rb.velocity = Vector2.zero;
                impulseBool = false; 
            }
        }
    }

    public void Knockback()
    {
        if (!vulnerable || isDead)
        {
            return;
        }
        rb.velocity = new Vector2(-transform.localScale.x, 1) * 2.0f;
        PlayHitSound();
        StartCoroutine(Invulnerable());
    }
    public void OnRespawn()
    {
        StartCoroutine(Invulnerable());
    }
    public void PlayHitSound()
    {
        PlaySound(getHitSound);
    }
    public void PlayHealSound()
    {
        PlaySound(getHealthSound);
    }
    public void PlayCoinSound()
    {
        PlaySound(getCoinSound);
    }


    private IEnumerator Invulnerable()
    {
        vulnerable = false;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        float blinkInterval = 0.1f;
        float timePassed = 0f;
        
        while (timePassed < vulnerableTime)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            timePassed += blinkInterval;
        }
        spriteRenderer.enabled = true;
        vulnerable = true;
        if (IsTouchingHazard())
        {
            Baqueta_health.instance.TakeDamage();
            Knockback();
        }
    }
    public bool IsTouchingHazard()
    {   
        if (baquetaCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            return true;
        }
        return false;
    }

    // Baqueta Sounds

    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    public void DisableBaqueta()
    {   horizontal = 0;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        speed = 0;
        isDead = true;
    }

    public void EnableBaqueta()
    {
        rb.gravityScale = originalGravityScale;
        isDead = false;
    }
    public void KillBaqueta()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        gameObject.SetActive(false);        
    }
}
