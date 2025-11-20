using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// La clase <c>LineBeatMarker</c> se encarga de gestionar el movimiento de cada marcador unico de ritmo
/// </summary>
public class LineBeatMarker : MonoBehaviour
{
    /// <summary>
    /// Booleano que indica si Baqueta se encuentra en modo cancion
    /// </summary>
    public bool isSongMode;
    /// <summary>
    /// Booleano que indica si el marcador se encuentra en la parte izquierda de la pantalla
    /// </summary>
    public bool isLeft;
    /// <summary>
    /// La posición del principio del contenedor del marcador
    /// </summary> 
    public Vector3 beginningPos;
    /// <summary>
    /// La posición original del marcador
    /// </summary>
    public Vector3 originalPos;
    /// <summary>
    /// La posición final del contenedor del marcador
    /// </summary>
    public Vector3 endPos;
    /// <summary>
    /// La referencia al marcador de ritmo
    /// </summary>
    public BeatMarkerUI beatMarkerUI; 
    private float elapsedTime;
    private const float duration = 2f;
    private Color originalColor;
    private Color attenuatedColor;
    private Image colorRenderer;
    /// <summary>
    /// La velocidad de movimiento del marcador
    /// </summary>
    public float speed = 200f;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        if (isLeft){
            beginningPos = beatMarkerUI.transform.position + new Vector3(436, 0, 0);
            endPos = beatMarkerUI.transform.position + new Vector3(48, 0, 0);
        } 
        else{
            beginningPos = beatMarkerUI.transform.position - new Vector3(436, 0, 0);
            endPos = beatMarkerUI.transform.position - new Vector3(48, 0, 0);
        }
        elapsedTime = 0f;
        colorRenderer = GetComponent<Image>();
        originalColor = colorRenderer.color;
        attenuatedColor = new Color(originalColor.r * 0.5f, originalColor.g * 0.5f, originalColor.b * 0.5f);
        
        if (isSongMode) colorRenderer.color = attenuatedColor;
    }

    // Update is called once per frame
    void Update()
    {   
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / duration; 
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, endPos, step);

        if (transform.position == endPos)
        {
            transform.position = beginningPos;
            elapsedTime = 0f;
        }
    }
    /// <summary>
    /// Método OnSongMode, se encarga de cambiar el color del marcador cuando Baqueta esta en modo canción
    /// </summary>
    public void OnSongMode()
    {
        if (isSongMode)
            colorRenderer.color = originalColor;
    }
    /// <summary>
    /// Método OnSongModeEnd, se encarga de cambiar el color del marcador cuando Baqueta sale del modo canción
    /// </summary> 
    public void OnSongModeEnd()
    {
        if (isSongMode)
        colorRenderer.color = attenuatedColor;
    }
    /// <summary>
    ///  Método ResetPosition, se encarga de resetear la posición del marcador cuando el evento OnBeat es llamado
    /// </summary>
    public void ResetPosition()
    {
        transform.position = originalPos;
    }
}
