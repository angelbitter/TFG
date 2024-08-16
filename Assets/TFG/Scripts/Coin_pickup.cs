using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin_pickup : MonoBehaviour
{
    private Animator animator;
    private bool collected = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !collected)
        {
            LevelController.instance.points += 100;
            animator.Play("PickedUp");
            collected = true;
            Baqueta_movement.instance.PlayCoinSound();
        }
    }
    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}
