using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround_image : MonoBehaviour
{
    public float amplitude;
    public float frequency;

    private Vector3 StartPosition;

    void Start()
    {
        StartPosition = transform.position;
    }

    void Update()
    {
        float yOffset = amplitude * Mathf.Sin(Time.time * frequency);
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
