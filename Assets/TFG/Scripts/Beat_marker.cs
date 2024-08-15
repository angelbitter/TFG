using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class Beat_marker : MonoBehaviour
{

    [SerializeField] private float PulseSize = 1.15f;
    [SerializeField] private float ReturnSpeed = 5f;
    
    private Vector3 OriginalScale;
    private int beatCounter;
    public UnityEvent[] beatEvents;

    void Start()
    {
        OriginalScale = transform.localScale;
        beatCounter = 0;
    }
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale,OriginalScale, Time.deltaTime * ReturnSpeed);
    }

    public void Pulse()
    {
        transform.localScale = OriginalScale * PulseSize;
    }

    public void CountBeatsOnSongMode(){
        if(beatCounter == 7)
            beatCounter = 0;
        else
            beatCounter++;
        beatEvents[beatCounter].Invoke();
    }
}
