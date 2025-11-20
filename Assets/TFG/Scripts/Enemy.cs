using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// La clase <c>Enemy</c> se encarga de gestionar a cada enemigo en el nivel
/// </summary>
/// <remarks>
/// Cada enemigo tiene un comportamiento diferente, algunos se mueven por tierra y otros por aire
/// </remarks>
public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;
    /// <summary>
    /// La fuerza con la que el enemigo salta
    /// </summary>
    public float jump;
    /// <summary>
    /// La amplitud a la que oscila un enemigo aereo
    /// </summary>
    public float flyAmplitude;
    /// <summary>
    /// La frecuencia a la que oscila un enemigo aereo
    /// </summary>
    public float flyFrequency;
    /// <summary>
    /// Referencia al collider del enemigo
    /// </summary>
    public CapsuleCollider2D colliderEnemy2;
    private bool isDead;
    /// <summary>
    /// La velocidad a la que se mueve el enemigo
    /// </summary>
    public float speed;
    /// <summary>
    /// Booleano que indica si el enemigo se mueve por tierra o por aire
    /// </summary>
    public bool isLand;
    /// <summary>
    /// Puntos entre los que el enemigo de tierra se mueve
    /// </summary>
    public Transform leftPoint, rightPoint;
    private Vector3 startPosition;
    private bool rightDirection = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        if (isLand)
        {
            leftPoint.parent = null;
            rightPoint.parent = null;
        }
        colliderEnemy2 = GetComponent<CapsuleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        isDead = false;
    }
    void Update()
    {
        if (isLand)
        {
            MovementLand();
        }else
        {
            MovementAir();
        }
        
    }

    private void MovementLand()
    {
        if (rightDirection)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            sr.flipX = true;
            if (transform.position.x > rightPoint.position.x)
            { 
                rightDirection = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            sr.flipX = false;
            if (transform.position.x < leftPoint.position.x)
            {
                rightDirection = true;
            }
        }
    }
    private void MovementAir()
    {
        
        float yOffset = Mathf.Sin(Time.time * flyFrequency) * flyAmplitude;
        transform.position = startPosition + new Vector3(0, yOffset, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && Baqueta_movement.instance.impulseBool)
        {
            Death();
            Baqueta_movement.instance.Impulse2();
            return;
        }
        
        if (collision.gameObject.tag == "SoundWave")
        {
            Death();
            Sound_wave_script soundWave = collision.gameObject.GetComponent<Sound_wave_script>();
            if (soundWave != null)
            {
                soundWave.OnDestroyWave();
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            Baqueta_health.instance.TakeDamage();  
            if(Baqueta_health.instance.health >= 0)
            {
                Baqueta_movement.instance.Knockback();
            }
        }
    }
    /// <summary>
    /// Método Death, se encarga de destruir al enemigo y sumar puntos al jugador
    /// </summary>
    public void Death()
    {
        colliderEnemy2.enabled = false;
        rb.velocity = Vector2.zero;
        speed = 0;
        
        animator.Play("DestroyItem");
        LevelController.instance.enemyPoints++; 
        isDead = true;
        flyAmplitude = 0;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
    /// <summary>
    /// Método OnBeat, se encarga de hacer saltar al enemigo en el momento del beat
    /// </summary>
    public void OnBeat()
    {   if(!isDead){
        animator.speed = 0;
        rb.velocity = Vector2.zero;
        speed = 0;
        rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
        StartCoroutine(OnBeatCoroutine());
        }
    }
    IEnumerator OnBeatCoroutine()
    {   
        yield return new WaitForSeconds(0.4f);
        animator.speed=1;
        if(!isDead){
            speed = 0.5f;
        }else{
            rb.velocity = Vector2.zero;
            speed = 0;
        }
    }
    public void DestroyItem()
    {        
        EnemiesController.instance.RemoveEnemy(this);
        Destroy(gameObject);
    }
}
