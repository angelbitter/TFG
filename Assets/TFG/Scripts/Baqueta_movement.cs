using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// La clase <c>Baqueta_movement</c> se encarga de gestionar el movimiento principal de Baqueta
/// </summary>
/// <remarks>
/// Esta clase también se encarga de gestionar las habilidades rítmicas, al igual que la interacción con el entorno
/// </summary>
public class Baqueta_movement : MonoBehaviour
{
    /// <summary>
    /// La instancia de la clase Baqueta_movement
    /// </summary>
    public static Baqueta_movement instance;
    private Rigidbody2D rb;

    /// <summary>
    /// El GameObject que representa la onda sonora en el entorno de Unity
    /// </summary>
    public GameObject soundWave;
    private Collider2D baquetaCollider;
    private float horizontal;
    private bool isGrounded;
    private bool wasFlying;
    private bool beatTriggered;
    private bool failBeatTriggered;

    /// <summary>
    /// Un booleano que indica si Baqueta ha sido impulsado, importante para manejar colisiones con otros elementos
    /// </summary>
    public bool impulseBool;
    private float originalGravityScale;
    private bool song;
    private int songResult = 0;
    
    private int note = 0;

    [SerializeField] private UnityEvent showWrongVFX;
    [SerializeField] private UnityEvent showRightVFX;

    /// <summary>
    /// La velocidad en cada momento de Baqueta
    /// </summary>
    public float speed;
    /// <summary>
    /// La velocidad maxima de Baqueta
    /// </summary>
    public float maxSpeed = 1.5f;
    /// <summary>
    /// La aceleración en el suelo de Baqueta
    /// </summary>  
    public float acceleration = 10.0f;
    /// <summary>
    /// La aceleración en el aire de Baqueta
    /// </summary>
    public float airAcceleration = 5.0f;
    private float jumpForce = 2.5f;
    public float impulseForce = 3f;
    public Vector2 impulseAngle = new Vector2(1, 1);
    private Vector2 originalColliderSize;
    private Vector2 impulseColliderSize = new Vector2(0.2f, 0.25f);
    /// <summary>
    /// Game Object que representa el punto de contacto con el suelo
    /// </summary>
    public Transform groundCheck;
    /// <summary>
    /// La máscara de capas que representa el suelo para permitir a Baqueta saltar
    /// </summary>
    public LayerMask whatIsGround;    
    private Vector3 originalScale;
    /// <summary>
    /// Un booleano que indica si Baqueta está escuchando un tutorial sonoro
    /// </summary>
    public bool Listening = false;
    /// <summary>
    /// Un booleano que indica la dirección de la burbuja de interacción
    /// </summary>
    public bool interactionBubbleDirection = false;
    [SerializeField] private float pulseSize = 1.15f;
    [SerializeField] private float returnSpeed = 5f;
    
    public AudioSource audioSource;     public AudioSource loopingSource;
    public AudioClip landAudio;         public AudioClip onRightSongAudio;
    public AudioClip onRightBeatAudio;  public AudioClip onWrongSongAudio;
    public AudioClip onWrongBeatAudio;  public AudioClip getHitSound;
    public AudioClip impulseAudio;      public AudioClip jumpAudio;
    public AudioClip soundWaveAudio;    public AudioClip soundWaveEndAudio;
    public AudioClip getCoinSound;      public AudioClip getHealthSound;
    public AudioClip bounceSound;       public AudioClip hitEnemySound;

    private Animator animator;

    /// <summary>
    /// Refetencia al elemento Beat_manager
    /// </summary>
    public Beat_manager beatManager;

    /// <summary>
    /// Un booleano que indica si Baqueta es vulnerable
    /// </summary>
    public bool vulnerable = true;
    /// <summary>
    /// El tiempo que Baqueta es invulnerable tras recibir daño
    /// </summary>
    public float vulnerableTime = 1.5f;
    /// <summary>
    /// Un booleano que indica si Baqueta ha muerto
    /// </summary>
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
        originalColliderSize = baquetaCollider.bounds.size;
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
        if(isDead)
        {
            return;
        }
        horizontal = Input.GetAxisRaw("Horizontal");
        
        if (!song){
            if(horizontal < 0 && isGrounded)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                originalScale = transform.localScale;
                impulseAngle = new Vector2(-1f, 1f);
                interactionBubbleDirection = false;
            }else if(horizontal > 0 && isGrounded)
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                originalScale = transform.localScale;
                impulseAngle = new Vector2(1f, 1f);
                interactionBubbleDirection = true;
            }

            if (!(isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.06f, whatIsGround)))
            {
                wasFlying = true;
            }else
            
            if(isGrounded && wasFlying)
            {
                PlaySound(landAudio);
                wasFlying = false;
                impulseBool = false;
                baquetaCollider.GetComponent<BoxCollider2D>().size = originalColliderSize;
            }

            if (Input.GetButtonDown("Jump")  && isGrounded)
            {
                Jump();
            }
            if (Input.GetButtonDown("Fire1") && !failBeatTriggered && !beatTriggered)
                {
                    //action button - start of SongMode
                    note=0;
                    beatManager.CheckSongMode();
                }
            }
            else {
                if (Input.GetButtonDown("Fire1") && song)
                {
                    //action button - song Mode Beats
                    beatManager.CheckSongModeBeat( note);
                    note++;
                }  
            }
        
    }
    /// <summary>
    /// Método Jump, se encarga de hacer saltar a Baqueta cuando el jugador pulsa el botón de salto
    /// </summary>
    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        PlaySound(jumpAudio);
    }
    /// <summary>
    /// Método Jump2, se encarga de hacer saltar a Baqueta cuando golpea a un enemigo
    /// </summary>
    public void Jump2()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        PlaySound(hitEnemySound);
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
                //air movement
                if (horizontal != 0.0f){
                    if (horizontal > 0 )
                        speed = Mathf.Min(speed + airAcceleration * Time.deltaTime, maxSpeed - 0.5f);
                    else
                        speed = Mathf.Max(speed - airAcceleration * Time.deltaTime, -maxSpeed + 0.5f);
                }else{
                    speed = Mathf.MoveTowards(speed, 0, airAcceleration * Time.deltaTime);
                }
            }
            rb.velocity = new Vector2(speed , rb.velocity.y);
        }
    }
    /// <summary>
    /// Método OnRightBeat, se lanza cuando el Beat_manager confirma que una pulsación de botón ha sido correcta, activa el Modo Canción
    /// </summary>
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
        impulseBool = false;
     }
     /// <summary>
     /// Método OnRightBeatSongMode, se lanza cuando el Beat_manager confirma que una pulsación de botón ha sido correcta en el Modo Canción, cuenta como una única nota correcta
     /// </summary>
     public void OnRightBeatSongMode(){
        showRightVFX.Invoke();
        PlaySound(onRightBeatAudio);
        Pulse();
     }

    /// <summary>
    /// Método OnWrongBeat, se lanza cuando el Beat_manager confirma que una pulsación de botón ha sido incorrecta, haciendo que Baqueta falle
    /// </summary>
    public void OnWrongBeat()
    {
        failBeatTriggered = true;
        PlaySound(onWrongBeatAudio);
        showWrongVFX.Invoke();
        speed = 0;
        StartCoroutine(ResetFailBeat());
    }
    /// <summary>
    /// Método OnWrongBeatSongMode, se lanza cuando el Beat_manager confirma que una pulsación de botón ha sido incorrecta en el Modo Canción, causando que la acción rítmica sea incorrecta
    /// </summary>
    public void OnWrongBeatSongMode(){
        PlaySound(onWrongBeatAudio);
        songResult = 0;
    }

    /// <summary>
    /// Método SongModeEnd, se lanza cuando el Beat_manager confirma que el Modo Canción ha terminado, activando la acción rítmica correspondiente
    /// </summary>
    public void SongModeEnd()
    {
        song = false;
        note = 0;
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

    /// <summary>
    /// Método CorrectImpulse, se lanza cuando el Beat_manager confirma que el jugador ha realizado la acción rítmica de Impulso correctamente
    /// </summary>
    public void CorrectImpulse()
    {
        songResult = 1;
    }

    /// <summary>
    /// Método CorrectSoundWave, se lanza cuando el Beat_manager confirma que el jugador ha realizado la acción rítmica de SoundWave correctamente
    /// </summary
    public void CorrectSoundWave()
    {
        songResult = 2;
    }

    /// <summary>
    /// Método Impulse, se lanza cuando el jugador realiza la acción rítmica de Impulso, que aplicará una fuerza a Baqueta en la dirección que este mirando
    /// </summary>
    public void Impulse()
    {   
        baquetaCollider.GetComponent<BoxCollider2D>().size = impulseColliderSize;
        impulseBool = true;
        if (baquetaCollider.IsTouchingLayers(LayerMask.GetMask("Obstacles")))
        {
            Collider2D[] results = new Collider2D[1];
            int numColliders = baquetaCollider.OverlapCollider(new ContactFilter2D().NoFilter(), results);
            Collider2D other = numColliders > 0 ? results[0] : null;
            Sound_barrier soundBarrier = other.gameObject.GetComponent<Sound_barrier>();
            if (soundBarrier != null)
            {
                soundBarrier.DeactivateCollision();
                soundBarrier.TakeDamage();
            }
        }
        rb.velocity = impulseAngle * impulseForce;
        loopingSource.PlayOneShot(impulseAudio);
    }
    /// <summary>
    /// Método Impulse2, Este metodo se llama cuando Baqueta rebota en un obstáculo y se impulsa con un sonido de rebote
public void Impulse2()
    {   
        baquetaCollider.GetComponent<BoxCollider2D>().size = impulseColliderSize;
        impulseBool = true;
        rb.velocity = impulseAngle * impulseForce;
        loopingSource.PlayOneShot(bounceSound);
    }

    /// <summary>
    /// Método ShootSoundWave, se lanza cuando el jugador realiza la acción rítmica de SoundWave, creando una onda sonora que daña a los enemigos
    /// </summary>
    public void ShootSoundWave()
    {
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        speed = 0;
        loopingSource.PlayOneShot(soundWaveAudio);
        StartCoroutine(ResetShootSoundwave());
    }

    /// <summary>
    /// Método OnSoundWaveDestruction, se lanza cuando la onda sonora creada por Baqueta es destruida, parando el sonido que estaba realizando
    /// </summary>
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
        GameObject soundWaveInst = Instantiate(soundWave, transform.position + direction * 0.2f, Quaternion.identity) as GameObject;
        soundWaveInst.GetComponent<Sound_wave_script>().SetDirection(direction);
        
        rb.gravityScale = originalGravityScale; 
    
    }

    /// <summary>
    /// Método Pulse, se encarga de hacer que Baqueta se expanda cuando pulsa el tambor al ritmo en el SongMode
    /// </summary>
    public void Pulse()
    {
        transform.localScale = originalScale * pulseSize;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && (!isGrounded|| impulseBool))
        {
            speed = 0;if (impulseBool)
            {
                impulseBool = false;
                baquetaCollider.GetComponent<BoxCollider2D>().size = originalColliderSize;
                rb.velocity = Vector2.zero;
            }
        }
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("SoundBarrier") && impulseBool)
        {
            
            Sound_barrier soundBarrier = other.gameObject.GetComponent<Sound_barrier>();
            if (soundBarrier != null)
            {
                soundBarrier.DeactivateCollision();
                soundBarrier.TakeDamage();
            }
        }
    }

    /// <summary>
    /// Método Knockback, se lanza cuando Baqueta recibe daño, aplicando una fuerza en la dirección contraria a la que mira
    /// </summary>
    public void Knockback()
    {
        if (!vulnerable || isDead)
        {
            return;
        }
        if (song)
        {
            songResult = 0;
            SongModeEnd();
        }
        rb.velocity = new Vector2(-transform.localScale.x, 1) * 2.0f;
        HitBox.instance.colliderHitBox.enabled = false;
        PlayHitSound();
        StartCoroutine(Invulnerable());

    }
    /// <summary>
    /// Método OnRespawn, se lanza cuando Baqueta reaparece tras caer por un precipicio, haciéndola invulnerable durante un tiempo
    /// </summary>
    public void OnRespawn()
    {
        StartCoroutine(Invulnerable());
    }
    /// <summary>
    /// Método PlayHitSound, se encarga de reproducir el sonido de daño al recibir un golpe, es accesible desde otras clases
    /// </summary>
    public void PlayHitSound()
    {
        PlaySound(getHitSound);
    }
    /// <summary>
    /// Método PlayHitSound, se encarga de reproducir el sonido de curarse, es accesible desde otras clases
    /// </summary>
    public void PlayHealSound()
    {
        PlaySound(getHealthSound);
    }
    /// <summary>
    /// Método PlayHitSound, se encarga de reproducir el sonido de coger una moneda, es accesible desde otras clases
    /// </summary>
    public void PlayCoinSound()
    {
        PlaySound(getCoinSound);
    }

    /// <summary>
    /// Método PlayHitSound, se encarga de reproducir el sonido que marca las corcheas en el Modo Canción
    /// </summary>
    public void PlaySongModeBeat()
    {
        audioSource.PlayOneShot(onRightBeatAudio);
    }
    /// <summary>
    /// Método PlayHitSound, se encarga de reproducir el sonido de canción correcta
    /// </summary>
    public void PlaySongModeBeat2()
    {
        audioSource.PlayOneShot(onRightSongAudio);
    }
    /// <summary>
    /// Método SetListening, se encarga de activar o desactivar el booleano Listening, que indica si Baqueta está escuchando un tutorial sonoro
    /// </summary>
    public void SetListening(bool value)
    {
        Listening = value;
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
        HitBox.instance.colliderHitBox.enabled = true;
        if (IsTouchingHazard())
        {
            Baqueta_health.instance.TakeDamage();
            Knockback();
        }
    }
    /// <summary>
    /// Método IsTouchingHazard, se encarga de comprobar si Baqueta está tocando un peligro una vez acaba su tiempo de invulnerabilidad
    /// </summary>
    /// <returns>
    /// Devuelve true si su collider está tocando una capa de peligro, false en caso contrario
    /// </returns>
    public bool IsTouchingHazard()
    {   
        if (baquetaCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            return true;
        }
        return false;
    }

    // Baqueta Sounds

    /// <summary>
    /// Método PlaySound, se encarga de reproducir un sonido en el AudioSource de Baqueta
    /// </summary>
    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    /// <summary>
    /// Método DisableBaqueta, se encarga de desactivar a Baqueta, parando su movimiento y gravedad
    /// </summary>
    /// <remarks>
    /// Se utiliza cuando Baqueta muere, cae por un precipicio o se cambia de escena
    /// </remarks>
    public void DisableBaqueta()
    {   horizontal = 0;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        speed = 0;
        isDead = true;
    }

    /// <summary>
    /// Método EnableBaqueta, se encarga de activar a Baqueta, reanudando su movimiento y gravedad
    /// </summary>
    public void EnableBaqueta()
    {
        rb.gravityScale = originalGravityScale;
        isDead = false;
    }

    /// <summary>
    /// Método KillBaqueta, se encarga de hacer desaparecer a Baqueta una vez su animacion de muerte ha terminado
    /// </summary> 
    public void KillBaqueta()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        gameObject.SetActive(false);        
    }
}
