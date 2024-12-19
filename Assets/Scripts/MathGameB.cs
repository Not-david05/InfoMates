using UnityEngine;
using TMPro;
using System.Collections;

public class MathGameM : MonoBehaviour
{
    public GameObject mathUI; // Interfaz de la operación matemática
    public GameObject answerPanel; // Panel donde se muestra la respuesta
    public TextMeshProUGUI textHint; // Texto para mostrar pistas o mensajes al jugador
    public TMP_InputField answerInputField; // TMP_InputField para que el jugador escriba la respuesta
    public Transform player; // Referencia al transform del jugador
    public TextMeshProUGUI externalTextToDestroy; // Referencia al objeto TextMeshPro externo
    public float interactionDistance = 3f; // Distancia mínima para interactuar
    public float delayBeforeClosing = 2f; // Tiempo de espera antes de cerrar el panel (en segundos)

    private CanvasGroup mathUICanvasGroup; // CanvasGroup para controlar la interactividad del panel
    private float correctAnswer; // Respuesta correcta de la operación (la solución más pequeña)
    private bool isInteractionLocked = false; // Bloquear interacciones después de resolver una operación
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
        mathUI.SetActive(false); // Asegúrate de que la interfaz esté desactivada al inicio
        textHint.gameObject.SetActive(false); // Ocultar el texto de pistas al inicio
        answerInputField.text = ""; // Limpiar el InputField al inicio
        answerInputField.onEndEdit.AddListener(CheckAnswer); // Vincular el evento al método CheckAnswer
    }

    private void Update()
    {
        
        // Bloquear interacción si está en espera
        if (isInteractionLocked)
            return;

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= interactionDistance && Input.GetKeyDown(KeyCode.E))
        {
            // Mostrar la interfaz matemática y bloquear el tiempo
            mathUI.SetActive(true);
            Time.timeScale = 0;
            EnablePanelInteraction(true);

            // Generar un nuevo problema matemático
            GenerateMathProblem();

            answerInputField.ActivateInputField(); // Activar el InputField para que el jugador escriba directamente
        }
    }

    private void GenerateMathProblem()
    {
        answerPanel.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        // Generar coeficientes para una ecuación de segundo grado
        int a, b, c;

        // Asegurarse de que 'a', 'b' y 'c' no sean cero
        do
        {
            a = Random.Range(1, 10); // Coeficiente principal (no cero)
            b = Random.Range(-10, 10); // Coeficiente lineal
            c = Random.Range(-10, 10); // Término independiente
        } while (b == 0 || c == 0);

        // Calcular el discriminante
        float discriminant = b * b - 4 * a * c;

        TMPro.TextMeshProUGUI problemText = mathUI.transform.Find("MathProblemText").GetComponent<TextMeshProUGUI>();
        if (problemText != null)
        {
            problemText.text = $"{a}x² + {b}x + {c} = 0. Encuentra la solución más pequeña (hasta 2 decimales).";
        }
        else
        {
            Debug.LogWarning("No se encontró el objeto TextMeshPro para la operación matemática.");
        }

        if (discriminant >= 0)
        {
            // Calcular soluciones reales
            float solution1 = (-b - Mathf.Sqrt(discriminant)) / (2 * a);
            float solution2 = (-b + Mathf.Sqrt(discriminant)) / (2 * a);
            correctAnswer = Mathf.Min(solution1, solution2); // Seleccionar la solución más pequeña
        }
        else
        {
            // Sin soluciones reales
            correctAnswer = float.NaN;
        }

        textHint.gameObject.SetActive(false); // Asegurarse de que el texto de hint esté oculto al inicio
    }


    public void CheckAnswer(string playerInput)
    {
        if (isInteractionLocked)
            return; // Evitar verificar respuestas si la interacción está bloqueada

        

        // Reemplazar comas por puntos para admitir entrada con comas
        playerInput = playerInput.Replace(',', '.');

        if (float.IsNaN(correctAnswer))
        {
            textHint.gameObject.SetActive(true); // Mostrar el texto de pistas
            textHint.text = "Esta ecuación no tiene soluciones reales.";
            textHint.color = Color.yellow;
            answerPanel.GetComponent<UnityEngine.UI.Image>().color = Color.yellow;
            StartCoroutine(ClosePanelWithDelay(false)); // Cerrar el panel inmediatamente
            return;
        }

        if (float.TryParse(playerInput, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float playerAnswer))
        {
            if (Mathf.Abs(playerAnswer - correctAnswer) <= 0.01f) // Permitir un margen de error pequeño
            {
                textHint.gameObject.SetActive(true); // Mostrar el texto de pistas
                // Respuesta correcta
                textHint.text = "¡Respuesta correcta!";
                textHint.color = Color.green; // Cambiar el color del texto a verde
                answerPanel.GetComponent<UnityEngine.UI.Image>().color = Color.green;

                if (externalTextToDestroy != null) // Verificar si se asignó un texto externo
                {
                    Destroy(externalTextToDestroy.gameObject); // Destruir el objeto TextMeshPro externo
                }

                StartCoroutine(ClosePanelWithDelay(true));
            }
            
        }
        

        answerInputField.text = ""; // Limpiar el InputField después de verificar la respuesta
    }

    private IEnumerator ClosePanelWithDelay(bool destroyObject)
    {
        isInteractionLocked = true; // Bloquear interacciones
        EnablePanelInteraction(false); // Desactivar interactividad del panel

        yield return new WaitForSecondsRealtime(delayBeforeClosing);

        // Reanudar el tiempo
        Time.timeScale = 1;
        mathUI.SetActive(false); // Ocultar la interfaz matemática

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
        // Controlar la interactividad del panel a través del CanvasGroup
        if (mathUICanvasGroup != null)
        {
            mathUICanvasGroup.interactable = enable;
            mathUICanvasGroup.blocksRaycasts = enable;
        }
    }
}



