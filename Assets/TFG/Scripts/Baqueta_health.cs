using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Baqueta_health : MonoBehaviour
{
    public int health, maxHealth;
    public static Baqueta_health instance;
    public bool vulnerable = true;
    public float vulnerableTime = 1.5f;
    public AudioClip deadSound;
    public AudioSource audioSource;
    public GameObject DeathMenu;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        health = maxHealth; 
    }

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
            audioSource.PlayOneShot(deadSound);
            gameObject.GetComponent<Animator>().Play("Death");
        }else {
            gameObject.GetComponent<Animator>().Play("Hurt");
            StartCoroutine(Invulnerable());
        }
    }

    public void Heal()
    {
        health = health + 2;
        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }
    IEnumerator Invulnerable()
    {
        vulnerable = false;
        yield return new WaitForSeconds(vulnerableTime);
        vulnerable = true;
    }
}
