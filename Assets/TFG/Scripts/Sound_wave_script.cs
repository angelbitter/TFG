using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_wave_script : MonoBehaviour
{
    public Animator animator;
    public float Speed = 2f;
    public float acceleration = 2f;
    public float maxSpeed = 3f;
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
    public void DestroyWave()
    {
        Destroy(gameObject);
    }
}
