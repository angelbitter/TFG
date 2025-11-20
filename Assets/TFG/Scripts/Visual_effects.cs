using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// La clase <c>Visual_effects</c> se encarga de gestionar los efectos visuales de los aciertos y fallos de Baqueta
/// </summary>
public class Visual_effects : MonoBehaviour

{    protected Animator Animator;

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

    /// <summary>
    /// Método que se llama cuando Baqueta falla un beat o se equivoca de canción
    /// </summary>
    public void OnWrongBeat(){
        WrongBeatTriggered = true;
    }
    /// <summary>
    /// Método que se llama cuando Baqueta acierta un beat
    /// </summary>
    public void OnRightBeat(){
        RightBeatTriggered = true;
    }
}
