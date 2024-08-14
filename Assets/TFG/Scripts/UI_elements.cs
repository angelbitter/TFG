using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_elements : MonoBehaviour
{
    public static UI_elements instance;
    public Image life1, life2, life3;
    public Sprite fullLife, emptyLife, halfLife;

    private void Awake()
    {
        instance = this;
    }
    [SerializeField] GameObject PauseMenu; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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


}
