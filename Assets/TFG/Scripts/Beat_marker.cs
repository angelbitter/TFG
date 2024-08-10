using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat_marker : MonoBehaviour
{

    [SerializeField] private float PulseSize = 1.15f;
    [SerializeField] private float ReturnSpeed = 5f;
    
    public GameObject Baqueta;
    private Vector3 OriginalScale;
    public float minHeight = -0.16f;
    public float maxHeight = 1.6f;
    void Start()
    {
        OriginalScale = transform.localScale;
    }
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale,OriginalScale, Time.deltaTime * ReturnSpeed);
        
        Vector3 position = transform.position ;
        position.x = Baqueta.transform.position.x - 1.5f;
        position.y = Mathf.Clamp(Baqueta.transform.position.y, minHeight, maxHeight) + 0.6f;
        transform.position = position;
    }

    public void Pulse()
    {
        transform.localScale = OriginalScale * PulseSize;
    }
}
