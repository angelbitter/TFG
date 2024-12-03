using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_baqueta : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && Baqueta_movement.instance.vulnerable )
        {
            Baqueta_health.instance.TakeDamage();
            Baqueta_movement.instance.Knockback();
        }
    }
}
