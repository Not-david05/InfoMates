using UnityEngine;

public class MathGame : MonoBehaviour
{
    public GameObject mathUI; // Interfaz de la operaci�n matem�tica
    private int correctAnswer; // Respuesta correcta de la operaci�n

    private void Start()
    {
        mathUI.SetActive(false); // Aseg�rate de que la interfaz est� desactivada al inicio
        GenerateMathProblem(); // Generar problema matem�tico al inicio
    }

    private void Update()
    {
        // No es necesario manejar el texto de interacci�n aqu� ahora que se ha movido a otro script
        // Deber�amos centrarnos solo en la l�gica de interacci�n con "E" para el c�lculo matem�tico.
        if (Input.GetKeyDown(KeyCode.E)) // Presionar "E" para interactuar con el problema matem�tico
        {
            mathUI.SetActive(true); // Mostrar la interfaz de la operaci�n matem�tica
            Time.timeScale = 0; // Pausar el juego
        }
    }

    private void GenerateMathProblem()
    {
        int num1 = Random.Range(1, 10);
        int num2 = Random.Range(1, 10);
        correctAnswer = num1 + num2;

        TMPro.TextMeshProUGUI problemText = mathUI.transform.Find("MathProblemText").GetComponent<TMPro.TextMeshProUGUI>();
        problemText.text = $"{num1} + {num2} = ?";
    }

    public void CheckAnswer(string playerInput)
    {
        if (int.TryParse(playerInput, out int playerAnswer) && playerAnswer == correctAnswer)
        {
            Destroy(gameObject); // Destruir el objeto interactuable
            mathUI.SetActive(false);
            Time.timeScale = 1; // Reanudar el juego
        }
        else
        {
            TMPro.TextMeshProUGUI feedbackText = mathUI.transform.Find("FeedbackText").GetComponent<TMPro.TextMeshProUGUI>();
            feedbackText.text = "Respuesta incorrecta. Intenta de nuevo.";
        }
    }
}
