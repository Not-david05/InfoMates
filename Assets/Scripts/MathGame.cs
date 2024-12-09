using UnityEngine;

public class MathGame : MonoBehaviour
{
    public GameObject mathUI; // Interfaz de la operación matemática
    private int correctAnswer; // Respuesta correcta de la operación

    private void Start()
    {
        mathUI.SetActive(false); // Asegúrate de que la interfaz esté desactivada al inicio
        GenerateMathProblem(); // Generar problema matemático al inicio
    }

    private void Update()
    {
        // No es necesario manejar el texto de interacción aquí ahora que se ha movido a otro script
        // Deberíamos centrarnos solo en la lógica de interacción con "E" para el cálculo matemático.
        if (Input.GetKeyDown(KeyCode.E)) // Presionar "E" para interactuar con el problema matemático
        {
            mathUI.SetActive(true); // Mostrar la interfaz de la operación matemática
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
