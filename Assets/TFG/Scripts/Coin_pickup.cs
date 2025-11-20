using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// La clase <c>Coin_pickup</c> se encarga de gestionar la recogida de monedas por parte de Baqueta y comunicarlo al LevelController
///  </summary>
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
            LevelController.instance.coinsCollected++;
            UI_elements.instance.UpdateCoinDisplay();
            animator.Play("PickedUp");
            collected = true;
            Baqueta_movement.instance.PlayCoinSound();
        }
    }
    /// <summary>
    /// Método DestroyItem, se encarga de destruir la moneda una vez ha sido recogida y su animación de desaparecer haya terminado
    /// </summary>
    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}
