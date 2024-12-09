using UnityEngine;

public class UIFollowCamera : MonoBehaviour
{
    public Camera mainCamera; // Cámara a seguir
    public float distanceFromCamera = 10f; // Distancia de la cámara
    public Vector2 canvasSize = new Vector2(1920, 1080); // Tamaño del canvas (en píxeles, por ejemplo, 1920x1080)

    private RectTransform canvasRectTransform;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Si no se asigna una cámara, se utiliza la cámara principal.
        }

        canvasRectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (mainCamera != null && canvasRectTransform != null)
        {
            // Ajustar la posición del canvas para que esté frente a la cámara
            transform.position = mainCamera.transform.position + mainCamera.transform.forward * distanceFromCamera;

            // Asegurar que el Canvas siempre esté mirando hacia la cámara
            transform.LookAt(mainCamera.transform);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0); // Evitar rotación extra

            // Calcular el tamaño adecuado del Canvas en función de la distancia de la cámara y el campo de visión
            float cameraHeight = 2.0f * distanceFromCamera * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float cameraWidth = cameraHeight * mainCamera.aspect;

            // Escalar el canvas para que tenga el mismo tamaño que la cámara
            float scaleX = cameraWidth / canvasSize.x;
            float scaleY = cameraHeight / canvasSize.y;

            // Ajustar la escala del Canvas
            canvasRectTransform.localScale = new Vector3(scaleX, scaleY, 1);
        }
    }
}
