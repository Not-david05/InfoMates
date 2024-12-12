using UnityEngine;
using TMPro; // Aseg�rate de tener esta l�nea si usas TextMeshPro

public class InteractionHint : MonoBehaviour
{
    public TextMeshProUGUI interactionText; // Referencia al TextMeshProUGUI (UI)
    public string hintMessage = "Presiona 'E' para interactuar"; // Mensaje que quieres mostrar
    public float interactionRange = 5f; // Rango de distancia para activar la interacci�n
    private Transform playerTransform;  // Referencia al transform del jugador
    private bool isPlayerNearby = false; // Estado de si el jugador est� cerca
    public string interactableTag = "Puertas"; // La etiqueta que debe tener el objeto con el que se interact�a
    public GameObject objectToDestroy; 
    private void Start()
    {
        playerTransform = Camera.main.transform; // Obtener la c�mara principal, que generalmente es la del jugador
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false); // Asegurarse de que el texto de interacci�n est� desactivado al inicio
        }
    }

    private void Update()
    {
        // Comprobar si el objeto tiene la etiqueta correcta
        if (!gameObject.CompareTag(interactableTag)) return;

        // Calcular la distancia entre el jugador (c�mara) y el objeto que tiene este script
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // Comprobar si el jugador est� dentro del rango del objeto etiquetado como "Puertas"
        isPlayerNearby = distance <= interactionRange;

        // Mostrar el texto de interacci�n si el jugador est� cerca
        if (isPlayerNearby)
        {
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(true); // Mostrar el texto
                interactionText.text = hintMessage; // Cambiar el texto a lo que se ha definido
            }

            if (Input.GetKeyDown(KeyCode.E)) // Si el jugador presiona "E"
            {
                Interact();
            }
        }
        else
        {
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false); // Ocultar el texto si el jugador se aleja
            }
        }
    }

    private void Interact()
    {
        // Aqu� puedes realizar la acci�n que desees cuando el jugador interact�e
        // En este caso, solo desactivamos el texto
        Debug.Log("Interacci�n realizada con la puerta.");
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false); // Ocultar el texto de interacci�n despu�s de interactuar
        }
    }
}
