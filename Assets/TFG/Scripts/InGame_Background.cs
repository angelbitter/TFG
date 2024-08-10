using UnityEngine;
using System.Collections;
public class Fondo : MonoBehaviour
{
    public Transform baqueta;
    protected MeshRenderer meshRender;
    public float velocidadX1, velocidadX2, velocidadX3, velocidadX4, velocidadX5;    
    public float velocidadY1, velocidadY2, velocidadY3, velocidadY4, velocidadY5;
    public float minHeight = -0.2f;
    public float maxHeight = 1.6f;
    public bool first; 

    private 

    // Use this for initialization
    void Start ()
    {
        meshRender = GetComponent<MeshRenderer>();

    }
    // Update is called once per frame
    void Update ()
    {
        if (first)
            meshRender.transform.position = new Vector3(baqueta.position.x - 1.079f,
            Mathf.Clamp(baqueta.transform.position.y, minHeight, maxHeight), meshRender.transform.position.z);
        else
            meshRender.transform.position = new Vector3(baqueta.position.x + 1.079f,
            Mathf.Clamp(baqueta.transform.position.y, minHeight, maxHeight), meshRender.transform.position.z);

        float posY = Mathf.Clamp(baqueta.transform.position.y, minHeight, maxHeight);
        meshRender.materials[0].SetTextureOffset("_MainTex", new Vector2(baqueta.position.x * velocidadX1, posY * velocidadY1));
        meshRender.materials[1].SetTextureOffset("_MainTex", new Vector2(baqueta.position.x * velocidadX2, posY * velocidadY2));
        meshRender.materials[2].SetTextureOffset("_MainTex", new Vector2(baqueta.position.x * velocidadX3, posY * velocidadY3));
        meshRender.materials[3].SetTextureOffset("_MainTex", new Vector2(baqueta.position.x * velocidadX4, posY * velocidadY4));
        meshRender.materials[4].SetTextureOffset("_MainTex", new Vector2(baqueta.position.x * velocidadX5, posY * velocidadY5));
    }
}