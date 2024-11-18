using UnityEngine;
using TMPro; // Necesario para usar TextMeshPro

public class MathGame : MonoBehaviour
{
    public TMP_Text operationText; // Texto para mostrar la operación
    public TMP_InputField answerInput; // Input para la respuesta del jugador
    public TMP_Text scoreText; // Texto para mostrar el puntaje
    public TMP_Text feedbackText; // Texto para mostrar comentarios (opcional)

    private int score = 0; // Puntos del jugador
    private int correctAnswer; // Respuesta correcta de la operación

    void Start()
    {
        GenerateOperation(); // Generar la primera operación
        UpdateScore();
    }

    public void CheckAnswer()
    {
        if (int.TryParse(answerInput.text, out int playerAnswer))
        {
            if (playerAnswer == correctAnswer)
            {
                score++;
                feedbackText.text = "¡Correcto!";
            }
            else
            {
                feedbackText.text = "Incorrecto, intenta de nuevo.";
            }

            UpdateScore();
            GenerateOperation();
        }
        else
        {
            feedbackText.text = "Por favor, ingresa un número válido.";
        }

        answerInput.text = ""; // Limpiar el campo de texto
    }

    void GenerateOperation()
    {
        int num1 = Random.Range(1, 10); // Primer número
        int num2 = Random.Range(1, 10); // Segundo número
        correctAnswer = num1 + num2; // Calcula la respuesta correcta (puedes cambiar la operación)

        operationText.text = $"{num1} + {num2} = ?";
    }

    void UpdateScore()
    {
        scoreText.text = $"Puntaje: {score}";
    }
}

