using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// La clase <c>EnemiesController</c> se encarga de gestionar a todos los enemigos del nivel
/// </summary>
public class EnemiesController : MonoBehaviour
{
    /// <summary>
    /// La instancia de la clase EnemiesController
    /// </summary>
    public static EnemiesController instance;
    /// <summary>
    /// Una lista con todos los enemigos del nivel
    /// </summary>
    public List<Enemy> enemies;

    private void Awake()
    {
        instance = this;
    }
    /// <summary>
    /// Método OnPulse, se encarga de llamar al método OnBeat de todos los enemigos
    /// </summary>
    public void OnPulse()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.OnBeat();
        }
    }
    /// <summary>
    /// Método AddEnemy, se encarga de eliminar un enemigo de la lista de enemigos
    /// </summary> 
    public void RemoveEnemy(Enemy enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }
}
