using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_wave_script : MonoBehaviour
{
    public float Speed = 2f;
    private Rigidbody2D rb;
    private Vector2 Direction;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.velocity = Direction * Speed;
    }

    public void SetDirection(Vector2 direction)
    {
        Direction = direction;
        transform.localScale = new Vector3(direction.x, 1, 1);
    }

    public void OnDestroyWave()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if (collision.gameObject.CompareTag("SoundBarrier"))
        // {
        //     collision.gameObject.GetComponent<Barrier>().TakeDamage();
        //     Destroy(gameObject);
        // }
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
