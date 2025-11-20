using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// La clase <c>DeathZone</c> se encarga de gestionar la colisión de Baqueta con las zonas de muerte
/// </summary>
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
