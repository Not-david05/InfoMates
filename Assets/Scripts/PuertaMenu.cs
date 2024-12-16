using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CambiarLvl : MonoBehaviour
{
    public TextMeshProUGUI textHint; // Texto para mostrar pistas o mensajes al jugador
    public Transform player; // Referencia al transform del jugador
    public float interactionDistance = 3f; // Distancia m�nima para interactuar
    public float delayBeforeChange = 2f; // Tiempo de espera antes de cambiar de nivel (en segundos)
    private bool isInteractionLocked = false; // Bloquear interacciones despu�s de activar el cambio de nivel

    private void Start()
    {
        // Configurar valores iniciales
        textHint.gameObject.SetActive(false); // Ocultar el texto de pistas al inicio
    }

    private void Update()
    {
        // Bloquear interacci�n si est� en espera
        if (isInteractionLocked)
            return;

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= interactionDistance)
        {
            // Mostrar texto de interacci�n si el jugador est� cerca
            textHint.gameObject.SetActive(true);
            textHint.text = "Presiona E para cambiar de nivel.";
            textHint.color = Color.white;

            if (Input.GetKeyDown(KeyCode.E))
            {
                // Bloquear el tiempo y mostrar mensaje de cambio
                Time.timeScale = 0;
                StartLevelChange();
            }
        }
        else
        {
            // Ocultar texto de interacci�n si el jugador est� fuera de rango
            textHint.gameObject.SetActive(false);
        }
    }

    private void StartLevelChange()
    {
        textHint.text = "Cambiando al siguiente nivel...";
        textHint.color = Color.green;
        StartCoroutine(ChangeLevelWithDelay());
    }

    private IEnumerator ChangeLevelWithDelay()
    {
        isInteractionLocked = true; // Bloquear interacciones

        yield return new WaitForSecondsRealtime(delayBeforeChange);

        // Reanudar el tiempo
        Time.timeScale = 1;

        // Cambiar al siguiente nivel (aseg�rate de que "NextLevel" sea el nombre correcto de la escena)
        SceneManager.LoadScene("Niveles");
    }
}
