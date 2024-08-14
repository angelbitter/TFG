using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_baqueta : MonoBehaviour
{
    public float vulnerableTime = 1.5f;
    public bool vulnerable = true;
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
        if(other.gameObject.tag == "Player" && vulnerable)
        {
            Baqueta_health.instance.TakeDamage();
            Baqueta.GetComponent<Baqueta_movement>().Knockback();
            StartCoroutine(Invulnerable());
        }
    }

    IEnumerator Invulnerable()
    {
        vulnerable = false;
        yield return new WaitForSeconds(vulnerableTime);
        vulnerable = true;
    }
    
}
