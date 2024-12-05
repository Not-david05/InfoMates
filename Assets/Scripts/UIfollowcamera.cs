using UnityEngine;

public class UIFollowCamera : MonoBehaviour
{
    public Camera mainCamera; // C�mara a seguir
    public float distanceFromCamera = 10f; // Distancia de la c�mara
    public Vector2 canvasSize = new Vector2(1920, 1080); // Tama�o del canvas (en p�xeles, por ejemplo, 1920x1080)

    private RectTransform canvasRectTransform;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Si no se asigna una c�mara, se utiliza la c�mara principal.
        }

        canvasRectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (mainCamera != null && canvasRectTransform != null)
        {
            // Ajustar la posici�n del canvas para que est� frente a la c�mara
            transform.position = mainCamera.transform.position + mainCamera.transform.forward * distanceFromCamera;

            // Asegurar que el Canvas siempre est� mirando hacia la c�mara
            transform.LookAt(mainCamera.transform);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0); // Evitar rotaci�n extra

            // Calcular el tama�o adecuado del Canvas en funci�n de la distancia de la c�mara y el campo de visi�n
            float cameraHeight = 2.0f * distanceFromCamera * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float cameraWidth = cameraHeight * mainCamera.aspect;

            // Escalar el canvas para que tenga el mismo tama�o que la c�mara
            float scaleX = cameraWidth / canvasSize.x;
            float scaleY = cameraHeight / canvasSize.y;

            // Ajustar la escala del Canvas
            canvasRectTransform.localScale = new Vector3(scaleX, scaleY, 1);
        }
    }
}
