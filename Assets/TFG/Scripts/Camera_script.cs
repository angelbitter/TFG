using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// La clase <c>Camera_script</c> se encarga de seguir a Baqueta en el eje X y de limitar su movimiento en el eje Y
/// </summary> 
public class Camera_script : MonoBehaviour
{
    /// <summary>
    /// La referencia al objeto Baqueta
    /// </summary> 
    public GameObject Baqueta;
    /// <summary>
    /// La altura mínima a la que puede llegar la cámara
    /// </summary>
    public float minHeight = -0.16f;
    /// <summary>
    /// La altura máxima a la que puede llegar la cámara  
    /// </summary>
    public float maxHeight = 1.6f;

    void Update()
    {
        Vector3 position = transform.position ;
        position.x = Baqueta.transform.position.x;
        position.y = Mathf.Clamp(Baqueta.transform.position.y, minHeight, maxHeight);
        transform.position = position;
    }
}
