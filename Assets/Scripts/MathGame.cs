using UnityEngine;
using TMPro;
using System.Collections;

public class MathGame : MonoBehaviour
{
    public GameObject mathUI; // Interfaz de la operaci�n matem�tica
    public GameObject answerPanel; // Panel donde se muestra la respuesta
    public TextMeshProUGUI textHint; // Texto para mostrar pistas o mensajes al jugador
    public TMP_InputField answerInputField; // TMP_InputField para que el jugador escriba la respuesta
    public Transform player; // Referencia al transform del jugador
    public TextMeshProUGUI externalTextToDestroy; // Referencia al objeto TextMeshPro externo
    public float interactionDistance = 3f; // Distancia m�nima para interactuar
    public float delayBeforeClosing = 2f; // Tiempo de espera antes de cerrar el panel (en segundos)
    private CanvasGroup mathUICanvasGroup; // CanvasGroup para controlar la interactividad del panel
    private int correctAnswer; // Respuesta correcta de la operaci�n
    private bool isInteractionLocked = false; // Bloquear interacciones despu�s de resolver una operaci�n
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        // Obtener el CanvasGroup de la interfaz (o agregar uno si no existe)
        mathUICanvasGroup = mathUI.GetComponent<CanvasGroup>();
        if (mathUICanvasGroup == null)
        {
            mathUICanvasGroup = mathUI.AddComponent<CanvasGroup>();
        }

        // Configurar valores iniciales
        mathUI.SetActive(false); // Aseg�rate de que la interfaz est� desactivada al inicio
        textHint.gameObject.SetActive(false); // Ocultar el texto de pistas al inicio
        answerInputField.text = ""; // Limpiar el InputField al inicio
        answerInputField.onEndEdit.AddListener(CheckAnswer); // Vincular el evento al m�todo CheckAnswer
        
    }

    private void Update()
    {
       
        // Bloquear interacci�n si est� en espera
        if (isInteractionLocked)
            return;

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= interactionDistance && Input.GetKeyDown(KeyCode.E))
        {
            // Mostrar la interfaz matem�tica y bloquear el tiempo
            mathUI.SetActive(true);
            Time.timeScale = 0;
            EnablePanelInteraction(true);

            // Generar un nuevo problema matem�tico
            GenerateMathProblem();

            answerInputField.ActivateInputField(); // Activar el InputField para que el jugador escriba directamente
        }
    }

    private void GenerateMathProblem()
    {
        // Generar dos n�meros aleatorios para la operaci�n
        int num1 = Random.Range(1, 100);
        int num2 = Random.Range(1, 100);
        correctAnswer = num1 * num2;

        // Actualizar el texto de la operaci�n matem�tica
        TMPro.TextMeshProUGUI problemText = mathUI.transform.Find("MathProblemText").GetComponent<TextMeshProUGUI>();
        if (problemText != null)
        {
            problemText.text = $"{num1} X {num2} = ?";
        }
        else
        {
            Debug.LogWarning("No se encontr� el objeto TextMeshPro para la operaci�n matem�tica.");
        }

        // Restablecer textHint a su estado original (vac�o o predeterminado)
        textHint.gameObject.SetActive(false); // Asegurarse de que el texto de hint est� oculto al inicio
    }

    public void CheckAnswer(string playerInput)
    {
        if (isInteractionLocked)
            return; // Evitar verificar respuestas si la interacci�n est� bloqueada

        

        if (int.TryParse(playerInput, out int playerAnswer))
        {
            if (playerAnswer == correctAnswer)
            {
                textHint.gameObject.SetActive(true); // Mostrar el texto de pistas
                // Respuesta correcta
                textHint.text = "�Respuesta correcta!";
                textHint.color = Color.green; // Cambiar el color del texto a verde
                answerPanel.GetComponent<UnityEngine.UI.Image>().color = Color.green;

                if (externalTextToDestroy != null) // Verificar si se asign� un texto externo
                {
                    Destroy(externalTextToDestroy.gameObject); // Destruir el objeto TextMeshPro externo
                }

                StartCoroutine(ClosePanelWithDelay(true));
            }
           
        }
       

        answerInputField.text = ""; // Limpiar el InputField despu�s de verificar la respuesta
    }

    private IEnumerator ClosePanelWithDelay(bool destroyObject)
    {
       
        isInteractionLocked = true; // Bloquear interacciones
        EnablePanelInteraction(false); // Desactivar interactividad del panel

        yield return new WaitForSecondsRealtime(delayBeforeClosing);

        // Reanudar el tiempo
        Time.timeScale = 1;
        mathUI.SetActive(false); // Ocultar la interfaz matem�tica

        if (destroyObject)
        {
            animator.SetBool("Destruir", true);
            Destroy(gameObject, 0.54f);
            answerPanel.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        }

        // Restablecer el estado del textHint a su valor inicial
        textHint.gameObject.SetActive(false); // Ocultar el texto de hint nuevamente

        isInteractionLocked = false; // Permitir interacciones nuevamente
    }

    private void EnablePanelInteraction(bool enable)
    {
        // Controlar la interactividad del panel a trav�s del CanvasGroup
        if (mathUICanvasGroup != null)
        {
            mathUICanvasGroup.interactable = enable;
            mathUICanvasGroup.blocksRaycasts = enable;
        }
    }
}


