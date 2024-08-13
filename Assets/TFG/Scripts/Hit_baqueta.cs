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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Hit Baqueta");
            Baqueta_health.instance.TakeDamage();
            Baqueta.GetComponent<Baqueta_movement>().Knockback();
        }
    }
}
