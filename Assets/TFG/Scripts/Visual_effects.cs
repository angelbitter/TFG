using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visual_effects : MonoBehaviour

{
    public GameObject Baqueta;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
     void Update()
    {
        Vector3 position = transform.position ;
        position.x = Baqueta.transform.position.x;
        position.y = Baqueta.transform.position.y;
        transform.position = position;
    }
}
