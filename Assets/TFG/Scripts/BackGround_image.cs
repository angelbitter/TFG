using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// La clase <c>BackGround_image</c> se encarga de mover el fondo de la pantalla de inicio
/// </summary>
/// <remarks>
/// El movimiento es una oscilación vertical de la imagen de fondo
/// </remarks>
public class BackGround_image : MonoBehaviour
{
    /// <summary>
    /// La amplitud del movimiento
    /// </summary>
    public float amplitude;

    /// <summary>
    /// La frecuencia a la que se moverá la imagen
    /// </summary>
    public float frequency;

    /// <summary>
    /// La posición inicial de la imagen y por la que oscilará
    /// </summary>
    private Vector3 StartPosition;

    /// <summary>
    /// Método Start, se ejecuta al inicio del script
    /// </summary>
    void Start()
    {
        StartPosition = transform.position;
    }
    /// <summary> 
    /// Método Update, se ejecuta una vez por frame
    /// </summary>
    void Update()
    {
        float yOffset = amplitude * Mathf.Sin(Time.time * frequency);
        transform.position = StartPosition + new Vector3(0, yOffset, 0);

    }
    /// <summary>
    /// Método OnRectTransformDimensionsChange, se ejecuta cuando cambia el tamaño de la pantalla
    /// </summary>
    /// <remarks>
    /// Sólo es necesario cambiar la posición inicial de la imagen, ya que el canvas de Unity se encarga del redimensionado 
    /// </remarks>
    void OnRectTransformDimensionsChange()
    {
        UpdateStartPosition();
    }
    /// <summary>
    /// El método UpdateStartPosition se encarga de actualizar la posición inicial de la imagen
    /// </summary>
    private void UpdateStartPosition()
    {
        StartPosition = transform.position;
    }
}
