using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat_marker : MonoBehaviour
{

    [SerializeField] private float PulseSize = 1.15f;
    [SerializeField] private float ReturnSpeed = 5f;
    // [SerializeField] private bool UseBeat;
    
    public GameObject Baqueta;
    private Vector3 OriginalScale;
    // Start is called before the first frame update
    void Start()
    {
        OriginalScale = transform.localScale;
        // if (UseBeat){
        //     StartCoroutine(BeatPulse());
        // }
    }
    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale,OriginalScale, Time.deltaTime * ReturnSpeed);
        
        Vector3 position = transform.position ;
        position.x = Baqueta.transform.position.x - 1.0f;
        transform.position = position;
    }

    public void Pulse()
    {
        transform.localScale = OriginalScale * PulseSize;
        Debug.Log("Pulse");
    }

    private IEnumerator BeatPulse()
    {
        while (true)
        {
            Pulse();
        }
    }
}
