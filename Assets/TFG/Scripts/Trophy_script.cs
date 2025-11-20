using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// La clase <c>Trophy_script</c> se encarga de gestionar la meta del nivel
/// </summary>
public class Trophy_script : MonoBehaviour
{
    public Animator animator; 
    public Collider2D collider2Dtrophy;
    private Vector3 startPosition;
    private float trophyAmplitude = 0.05f;
    private float trophyFrequency = 1f;
    [SerializeField] GameObject WinScreen; 
     void Start()
    {
        startPosition = transform.position;
        animator = GetComponent<Animator>();
        collider2Dtrophy = GetComponent<Collider2D>();
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * trophyFrequency) * trophyAmplitude;
        transform.position = startPosition + new Vector3(0, yOffset, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            LevelController.instance.LevelComplete();
            animator.Play("PickedUp");
        }
    }
    public void DestroyItem()
    {
        Time.timeScale = 0f;
        Destroy(gameObject);
        WinScreen.SetActive(true);
    }
}
