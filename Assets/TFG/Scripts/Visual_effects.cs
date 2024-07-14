using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visual_effects : MonoBehaviour

{
    public GameObject Baqueta;
    protected Animator Animator;

    private bool WrongBeatTriggered;
    private bool RightBeatTriggered;

    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
     void Update()
    {
        Animator.SetBool("wrong", WrongBeatTriggered);
        Animator.SetBool("right", RightBeatTriggered);
        WrongBeatTriggered = false;
        RightBeatTriggered = false;
    }

    public void OnWrongBeat(){
        WrongBeatTriggered = true;
    }
    public void OnRightBeat(){
        RightBeatTriggered = true;
    }
}
