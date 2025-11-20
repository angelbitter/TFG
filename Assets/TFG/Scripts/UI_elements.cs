using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// La clase <c>UI_elements</c> se encarga de gestionar los elementos del HUD del jugador
/// </summary>
public class UI_elements : MonoBehaviour
{
    /// <summary>
    /// La instancia de la clase UI_elements
    /// </summary>
    public static UI_elements instance;
    /// <summary>
    /// Los iconos de vida de Baqueta
    /// </summary>
    public Image life1, life2, life3;
    /// <summary>
    /// Los sprites de la vida de Baqueta
    /// </summary>
    public Sprite fullLife, emptyLife, halfLife;
    /// <summary>
    /// El texto que muestra las monedas recogidas
    /// </summary>
    public TextMeshProUGUI coinText;
    [SerializeField] GameObject PauseMenu; 

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        UpdateCoinDisplay();
    }
    /// <summary>
    /// Método que se llama cuando la vida de baqueta cambia
    /// </summary>
    public void UpdateHealthDisplay()
    {
        switch (Baqueta_health.instance.health)
        {
            case 6:
                life1.sprite = fullLife;
                life2.sprite = fullLife;
                life3.sprite = fullLife;
                break;
            case 5:
                life1.sprite = halfLife;
                life2.sprite = fullLife;
                life3.sprite = fullLife;
                break;
            case 4:
                life1.sprite = emptyLife;
                life2.sprite = fullLife;
                life3.sprite = fullLife;
                break;
            case 3:
                life1.sprite = emptyLife;
                life2.sprite = halfLife;
                life3.sprite = fullLife;
                break;
            case 2:
                life1.sprite = emptyLife;
                life2.sprite = emptyLife;
                life3.sprite = fullLife;
                break;
            case 1:
                life1.sprite = emptyLife;
                life2.sprite = emptyLife;
                life3.sprite = halfLife;
                break;
            default:
                life1.sprite = emptyLife;
                life2.sprite = emptyLife;
                life3.sprite = emptyLife;
                break;
        }
    }
    /// <summary>
    /// Método que se llama cuando Baqueta recoge una moneda
    /// </summary>
    public void UpdateCoinDisplay()
    {
        coinText.text = LevelController.instance.coinsCollected.ToString();
    }

}
