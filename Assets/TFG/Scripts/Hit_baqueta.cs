using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// La clase <c>Hit_baqueta</c> se encarga de gestionar la colisión de Baqueta con los enemigos y hazards
/// </summary>
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
