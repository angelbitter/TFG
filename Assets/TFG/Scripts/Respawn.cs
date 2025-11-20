using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// La clase <c>Respawn</c> se encarga de gestionar un punto de respawn del nivel
/// </summary>
public class Respawn : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {  
            RespawnController.instance.SetRespawnPoint(transform.position);
        }
    }
}
