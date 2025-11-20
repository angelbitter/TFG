using UnityEngine;
using System.Collections;
/// <summary>
/// La clase <c>InGame_Background</c> se encarga de mover el fondo del nivel
/// </summary>
public class InGame_Background : MonoBehaviour
{
    /// <summary>
    /// La referencia al objeto Baqueta
    /// </summary>
    public Transform baqueta;
    protected MeshRenderer meshRender;
    /// <summary>
    /// La velocidad de movimiento del fondo en el eje X de cada capa de textura
    /// </summary>
    public float velocidadX1, velocidadX2, velocidadX3, velocidadX4, velocidadX5;    
    /// <summary>
    /// La velocidad de movimiento del fondo en el eje Y de cada capa de textura
    /// </summary>
    public float velocidadY1, velocidadY2, velocidadY3, velocidadY4, velocidadY5;
    /// <summary>
    /// La altura mínima a la que puede llegar el fondo
    /// </summary>
    public float minHeight = -0.2f;
    /// <summary>
    /// La altura máxima a la que puede llegar el fondo
    /// </summary>
    public float maxHeight = 1.6f;
    /// <summary>
    /// Booleano que indica si el fondo es el primero o el segundo, ya que por su tamaño se usan dos fondos
    /// </summary>
    public bool first; 

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