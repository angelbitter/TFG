using UnityEngine;
using UnityEngine.UI;

public class LineBeatMarker : MonoBehaviour
{
    public bool isSongMode;
    public bool isLeft;
    public Vector3 beginningPos;
    public Vector3 originalPos;
    public Vector3 endPos;
    public BeatMarkerUI beatMarkerUI; 
    private float elapsedTime;
    private const float duration = 2f;
    private Color originalColor;
    private Color attenuatedColor;
    private Image colorRenderer;
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
    public void OnSongMode()
    {
        if (isSongMode)
            colorRenderer.color = originalColor;
    }
    public void OnSongModeEnd()
    {
        if (isSongMode)
        colorRenderer.color = attenuatedColor;
    }
    public void ResetPosition()
    {
        transform.position = originalPos;
    }
}
