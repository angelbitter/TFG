using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Baqueta_health.instance.TakeDamage();
            Baqueta_movement.instance.impulseBool = false;
            Baqueta_movement.instance.PlayHitSound();
            if (Baqueta_health.instance.health > 0)
                LevelController.instance.Respawn();
        }
    }
}
