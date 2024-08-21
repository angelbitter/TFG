using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
   
    public static EnemiesController instance;
    public List<Enemy> enemies;

    private void Awake()
    {
        instance = this;
    }
    public void OnPulse()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.OnBeat();
        }
    }
    public void RemoveEnemy(Enemy enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }
}
