using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround_image : MonoBehaviour
{
    public float Amplitude;
    public float Frequency;

    private Vector3 StartPosition;

    void Start()
    {
        StartPosition = transform.position;
    }

    void Update()
    {
        float yOffset = Amplitude * Mathf.Sin(Time.time * Frequency);
        transform.position = StartPosition + new Vector3(0, yOffset, 0);

    }
    void OnRectTransformDimensionsChange()
    {
        UpdateStartPosition();
    }

    private void UpdateStartPosition()
    {
        StartPosition = transform.position;
    }
}
