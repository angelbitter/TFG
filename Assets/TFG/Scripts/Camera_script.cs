using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_script : MonoBehaviour
{

    public GameObject Baqueta;

    void Update()
    {
        Vector3 position = transform.position ;
        position.x = Baqueta.transform.position.x;
        transform.position = position;
    }
}
