using UnityEngine;

public class AlignUIToCamera : MonoBehaviour
{
    public Camera mainCamera;
    public RectTransform panelRectTransform;

    private void Update()
    {
        if (mainCamera != null && panelRectTransform != null)
        {
            // Mueve el panel a la posición de la cámara
            panelRectTransform.position = mainCamera.WorldToScreenPoint(mainCamera.transform.position + mainCamera.transform.forward * 5f);

            // Si necesitas que el panel mire siempre a la cámara:
            panelRectTransform.LookAt(mainCamera.transform);
        }
    }
}
