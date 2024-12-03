using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMarker : MonoBehaviour
{
    
    private float PulseSize = 1.15f;
    private float ReturnSpeed = 5f;
    private Vector3 OriginalScale;
    public bool startOrFinish;
    [SerializeField] private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        OriginalScale = transform.localScale;
    }
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale,OriginalScale, Time.deltaTime * ReturnSpeed);
    }
    public void Pulse()
    {
        transform.localScale = OriginalScale * PulseSize;
        animator.Play("Pulse");
        if(gameObject.GetComponentInParent<Beat_marker>().play){
            if (startOrFinish)
            {
                Baqueta_movement.instance.PlaySongModeBeat2();
            }
            else
            {
                Baqueta_movement.instance.PlaySongModeBeat();
            }
            }
    }
}
