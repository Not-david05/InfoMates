using UnityEngine;

public class MathUIManager : MonoBehaviour
{
    public MathGame interactableMath; // Referencia al objeto interactuable
    public TMPro.TMP_InputField inputField; // Campo de entrada para la respuesta

    public void SubmitAnswer()
    {
        if (interactableMath != null)
        {
            interactableMath.CheckAnswer(inputField.text);
            inputField.text = ""; // Limpiar el campo de entrada
        }
    }
}
