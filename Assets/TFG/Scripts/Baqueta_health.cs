using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// La clase <c>Baqueta_health</c> se encarga de gestionar la vida de Baqueta
/// </summary>
/// <remarks>
/// La clase se encarga de gestionar la vida de Baqueta, así como de su invulnerabilidad y de su muerte
/// </remarks>
public class Baqueta_health : MonoBehaviour
{
    /// <summary>
    /// La vida actual de Baqueta y su vida máxima
    /// </summary>
    public int health, maxHealth;
    /// <summary>
    /// La instancia de la clase Baqueta_health
    /// </summary>
    public static Baqueta_health instance;
    /// <summary>
    /// Un booleano que indica si Baqueta es vulnerable
    /// </summary>/// 
    public bool vulnerable = true;
    /// <summary>
    /// El tiempo que Baqueta es invulnerable tras recibir daño
    /// </summary>
    public float vulnerableTime = 1.5f;
    /// <summary>
    /// El sonido que se reproduce al morir Baqueta
    /// </summary>
    public AudioClip deadSound;
    /// <summary>
    /// El componente AudioSource de Baqueta
    /// </summary>
    public AudioSource audioSource;
    /// <summary>
    /// Referencia al menú de Game Over 
    public GameObject DeathMenu;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        health = maxHealth; 
    }

    /// <summary>
    /// Método TakeDamage, se encarga de reducir la vida de Baqueta
    /// </summary>
    public void TakeDamage()
    {
        if (!vulnerable)
        {
            return;
        }
        health--;
        UI_elements.instance.UpdateHealthDisplay();
        if(health <= 0)
        {
            DeathMenu.gameObject.SetActive(true);
            Baqueta_movement.instance.isDead = true;
            Baqueta_movement.instance.DisableBaqueta();
            audioSource.PlayOneShot(deadSound);
            gameObject.GetComponent<Animator>().Play("Death");
        }else {
            gameObject.GetComponent<Animator>().Play("Hurt");
            StartCoroutine(Invulnerable());
        }
    }

    /// <summary>
    /// Método Heal, se encarga de aumentar la salud de Baqueta cuando recoge una vida
    /// </summary>
    public void Heal()
    {
        health = health + 2;
        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }

    private IEnumerator Invulnerable()
    {
        vulnerable = false;
        yield return new WaitForSeconds(vulnerableTime);
        vulnerable = true;
    }
}
