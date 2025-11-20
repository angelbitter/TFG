using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// La clase <c>RespawnController</c> se encarga de gestionar los puntos de respawn de Baqueta
/// </summary>
public class RespawnController : MonoBehaviour
{
    /// <summary>
    /// Los puntos de respawn de Baqueta
    /// </summary>
    public Respawn[] respawns;
    /// <summary>
    /// El punto de respawn de Baqueta que esta activo en ese momento
    /// </summary>
    public Vector3 respawnPoint;
    /// <summary>
    ///  La instancia de la clase RespawnController
    /// </summary>
    public static RespawnController instance;
    
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        respawns = FindObjectsOfType<Respawn>();
        respawnPoint = Baqueta_movement.instance.transform.position;
    }
    /// <summary>
    /// Método que se llama cuando Baqueta atraviesa un punto de respawn
    /// </summary>
    /// <param name="point"> El punto de respawn que se guarda</param>
    public void SetRespawnPoint(Vector3 point)
    {
        respawnPoint = point;
    }
}
