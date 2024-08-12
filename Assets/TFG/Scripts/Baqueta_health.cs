using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baqueta_health : MonoBehaviour
{
    public int health, maxHealth;
    public static Baqueta_health instance;
    public bool vulnerable = true;
    
    public float hitAngle = -60f;
    private Vector2 hitDirection;

    public float hitForce;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth; 
        hitAngle = hitAngle * Mathf.Deg2Rad;
        float hitAngleInRad = hitAngle * Mathf.Deg2Rad;
        hitDirection = new Vector2(Mathf.Cos(hitAngleInRad), Mathf.Sin(hitAngleInRad));
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
        gameObject.GetComponent<Animator>().SetTrigger("Hit");
        gameObject.GetComponent<Rigidbody2D>().velocity = hitDirection * hitForce;
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
