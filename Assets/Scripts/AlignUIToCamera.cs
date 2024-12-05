using UnityEngine;

public class AlignUIToCamera : MonoBehaviour
{
    public Camera mainCamera;
    public RectTransform panelRectTransform;

    private void Update()
    {
        if (mainCamera != null && panelRectTransform != null)
        {
            // Mueve el panel a la posici�n de la c�mara
            panelRectTransform.position = mainCamera.WorldToScreenPoint(mainCamera.transform.position + mainCamera.transform.forward * 5f);

            // Si necesitas que el panel mire siempre a la c�mara:
            panelRectTransform.LookAt(mainCamera.transform);
        }
    }
}
