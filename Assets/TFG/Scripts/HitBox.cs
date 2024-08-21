using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public static HitBox instance;
    public BoxCollider2D colliderHitBox;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        colliderHitBox = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().colliderEnemy2.enabled = false;
            other.gameObject.GetComponent<Enemy>().Death();
            Baqueta_movement.instance.Jump2();
        }
        if (other.gameObject.tag == "Hazard" && Baqueta_movement.instance.impulseBool)
        {
            Baqueta_movement.instance.Impulse2();
        }
        if (other.gameObject.tag == "SoundBarrier" && Baqueta_movement.instance.impulseBool)
        { 
            other.gameObject.GetComponent<Sound_barrier>().TakeDamage();
        }
    }
}
