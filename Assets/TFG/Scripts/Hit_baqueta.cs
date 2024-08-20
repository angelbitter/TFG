using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_baqueta : MonoBehaviour
{
    public GameObject Baqueta;
    // Start is called before the first frame update
    void Start()
    {
        Baqueta = GameObject.Find("Baqueta");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && Baqueta_movement.instance.vulnerable && !Baqueta_movement.instance.impulseBool)
        {
            Baqueta_health.instance.TakeDamage();
            Baqueta.GetComponent<Baqueta_movement>().Knockback();
        }
        // else if(other.gameObject.tag == "Player" && Baqueta_movement.instance.impulseBool)
        // {
        //     Baqueta_movement.instance.Impulse();
        // }
    }
}
