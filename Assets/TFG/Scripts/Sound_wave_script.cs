using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_wave_script : MonoBehaviour
{
    public float Speed = 2f;
    public float acceleration = 2f;
    public float maxSpeed = 3f;
    public float lifeTime = 5f;
    private Rigidbody2D rb;
    private Vector2 Direction;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(TimeUntilDestroy());
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
        Baqueta_movement.instance.OnSoundWaveDestruction();
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("SoundBarrier"))
        {
            //collision.gameObject.GetComponent<Barrier>().TakeDamage();
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            OnDestroyWave();
        }
    }
}
