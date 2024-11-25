using UnityEngine;

public class MathGame : MonoBehaviour
{
    public GameObject mathUI; // Interfaz de la operaci�n matem�tica
    private bool isPlayerNearby = false;
    private int correctAnswer; // Respuesta correcta de la operaci�n

    private void Start()
    {
        mathUI.SetActive(false); // Aseg�rate de que la interfaz est� desactivada al inicio
        GenerateMathProblem();
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E)) // Presionar "E" para interactuar
        {
            mathUI.SetActive(true); // Mostrar la interfaz de la operaci�n matem�tica
            Time.timeScale = 0; // Pausar el juego
        }
    }

    private void GenerateMathProblem()
    {
        // Generar dos n�meros aleatorios para la operaci�n
        int num1 = Random.Range(1, 10);
        int num2 = Random.Range(1, 10);

        // Generar una suma (puedes cambiar el tipo de operaci�n)
        correctAnswer = num1 + num2;

        // Buscar el texto en la interfaz para mostrar la operaci�n
        TMPro.TextMeshProUGUI problemText = mathUI.transform.Find("MathProblemText").GetComponent<TMPro.TextMeshProUGUI>();
        problemText.text = $"{num1} + {num2} = ?";
    }

    public void CheckAnswer(string playerInput)
    {
        if (int.TryParse(playerInput, out int playerAnswer) && playerAnswer == correctAnswer)
        {
            // Respuesta correcta: destruir el objeto y cerrar la UI
            Destroy(gameObject);
            mathUI.SetActive(false);
            Time.timeScale = 1; // Reanudar el juego
        }
        else
        {
            // Respuesta incorrecta: mostrar un mensaje de error
            TMPro.TextMeshProUGUI feedbackText = mathUI.transform.Find("FeedbackText").GetComponent<TMPro.TextMeshProUGUI>();
            feedbackText.text = "Respuesta incorrecta. Intenta de nuevo.";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Detectar si el jugador est� cerca
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Detectar si el jugador se aleja
        {
            isPlayerNearby = false;
        }
    }
}

