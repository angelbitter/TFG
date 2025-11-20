using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// La clase <c>Sound_wave_script</c> se encarga de gestionar las ondas sonoras que Baqueta lanza
/// </summary>
public class Sound_wave_script : MonoBehaviour
{
    public Animator animator;
    /// <summary>
    ///  La velocidad de la onda sonora
    /// </summary>
    public float Speed = 2f;
    /// <summary>
    ///  La aceleración de la onda sonora
    /// </summary>
    public float acceleration = 2f;
    /// <summary>
    /// La velocidad máxima de la onda sonora
    /// </summary>
    public float maxSpeed = 3f;
    /// <summary>
    /// El tiempo de vida de la onda sonora
    /// </summary>
    public float lifeTime = 3f;
    private Rigidbody2D rb;
    private Vector2 Direction;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(TimeUntilDestroy());
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.velocity = Direction * Speed;
        Speed += acceleration * Time.fixedDeltaTime;
    }
    /// <summary>
    /// Método que se llama pata establecer la dirección de la onda sonora
    /// </summary>
    /// <param name="direction">La dirección de la onda sonora</param>
    public void SetDirection(Vector2 direction)
    {
        Direction = direction;
        transform.localScale = new Vector3(direction.x, 1, 1);
    }
    private IEnumerator TimeUntilDestroy()
    {
        yield return new WaitForSeconds(lifeTime);
        if (gameObject != null)
            OnDestroyWave();
    }
    /// <summary>
    /// Método que se llama cuando la onda sonora choca con algo
    /// </summary>
    public void OnDestroyWave()
    {
        Speed=0;
        acceleration=0;
        Baqueta_movement.instance.OnSoundWaveDestruction();
        animator.Play("WaveDestroy");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.velocity = Vector2.zero;
        if (collision.gameObject.CompareTag("SoundBarrier"))
        {
            Sound_barrier soundBarrier = collision.gameObject.GetComponent<Sound_barrier>();
            if (soundBarrier != null)
            {
                soundBarrier.TakeDamage();
            }
            OnDestroyWave();
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            OnDestroyWave();
        }
    }
    /// <summary>
    /// Método que se llama cuando la animación de destrucción de la onda sonora termina
    /// </summary> 
    public void DestroyWave()
    {
        Destroy(gameObject);
    }
}
