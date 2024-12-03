using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life_pickup : MonoBehaviour
{
    public Animator animator;
    private bool collected = false;
    private float lifeAmplitude = 0.05f;
    private float lifeFrequency = 1f;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * lifeFrequency) * lifeAmplitude;
        transform.position = startPosition + new Vector3(0, yOffset, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !collected && Baqueta_health.instance.health < Baqueta_health.instance.maxHealth)
        {
                Baqueta_health.instance.Heal();
                collected = true;
                UI_elements.instance.UpdateHealthDisplay();
                animator.Play("PickedUp");
                Baqueta_movement.instance.PlayHealSound();
        }
    }
    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}

