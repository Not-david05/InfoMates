using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Camera mainCamera;  // La cámara que vamos a seguir
    public Vector2 screenCorner = new Vector2(1, 1);  // Coordenadas para la esquina, (1,1) es la esquina superior derecha

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;  // Si no se asigna, toma la cámara principal
        }
    }

    void Update()
    {
        // Calculamos la posición de la esquina en el espacio de pantalla
        Vector3 screenPosition = new Vector3(screenCorner.x, screenCorner.y, mainCamera.nearClipPlane);

        // Convertimos las coordenadas de pantalla a mundo
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);

        // Movemos el sprite a la posición calculada
        transform.position = worldPosition;
    }
}
