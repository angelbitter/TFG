using UnityEngine;
using System.Collections;
public class Fondo : MonoBehaviour
{
    public Transform baqueta;
    protected MeshRenderer meshRender;
    public float velocidad1;
    public float velocidad2;
    public float velocidad3;
    public float velocidad4;
    public float velocidad5;
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
            meshRender.transform.position = new Vector3(baqueta.position.x - 1.079f, meshRender.transform.position.y, meshRender.transform.position.z);
        else
            meshRender.transform.position = new Vector3(baqueta.position.x + 1.079f, meshRender.transform.position.y, meshRender.transform.position.z);
        float offset1 = (baqueta.position.x * velocidad1) % 1;
        float offset2 = (baqueta.position.x * velocidad2) % 1;
        float offset3 = (baqueta.position.x * velocidad3) % 1;
        float offset4 = (baqueta.position.x * velocidad4) % 1;
        float offset5 = (baqueta.position.x * velocidad5) % 1;

        meshRender.materials[0].SetTextureOffset("_MainTex", new Vector2(offset1, 0));
        meshRender.materials[1].SetTextureOffset("_MainTex", new Vector2(offset2, 0));
        meshRender.materials[2].SetTextureOffset("_MainTex", new Vector2(offset3, 0));
        meshRender.materials[3].SetTextureOffset("_MainTex", new Vector2(offset4, 0));
        meshRender.materials[4].SetTextureOffset("_MainTex", new Vector2(offset5, 0));
    
    }
}