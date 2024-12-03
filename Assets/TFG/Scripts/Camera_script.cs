using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_script : MonoBehaviour
{
    public GameObject Baqueta;
    public float minHeight = -0.16f;
    public float maxHeight = 1.6f;

    void Update()
    {
        Vector3 position = transform.position ;
        position.x = Baqueta.transform.position.x;
        position.y = Mathf.Clamp(Baqueta.transform.position.y, minHeight, maxHeight);
        transform.position = position;
    }
}
