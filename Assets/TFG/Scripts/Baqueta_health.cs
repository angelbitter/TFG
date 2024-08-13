using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baqueta_health : MonoBehaviour
{
    public int health, maxHealth;
    public static Baqueta_health instance;
    public bool vulnerable = true;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage()
    {
        if (!vulnerable)
        {
            return;
        }
        health--;
        if(health <= 0)
        {
            gameObject.SetActive(false);

            
        }
        UI_elements.instance.UpdateHealthDisplay();
        gameObject.GetComponent<Animator>().Play("Hurt");
        StartCoroutine(Invulnerable());
    }

    public void Heal()
    {
        health++;
        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }
    IEnumerator Invulnerable()
    {
        vulnerable = false;
        yield return new WaitForSeconds(1f);
        vulnerable = true;
    }
}
