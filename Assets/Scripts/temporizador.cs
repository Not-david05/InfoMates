using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Referencia al texto de la UI
    public float startTimeInSeconds = 60f; // Tiempo inicial en segundos

    private float timeRemaining; // Tiempo restante en la cuenta regresiva
    private bool isTimerRunning = true; // Controla si el contador está activo

    [Header("Offset Settings")]
    //public Vector3 positionOffset = new Vector3(0f, 0f, 0f); 
    public Vector3 positionOffset = new Vector3(); // Desplazamiento en relación a la cámara

    private Camera mainCamera; // Referencia a la cámara principal

    public delegate void TimerEnded(); // Declaración de un delegado para eventos
    public static event TimerEnded OnTimerEnd; // Evento para cuando el tiempo se agote

    void Start()
    {
        mainCamera = Camera.main; // Obtén la cámara principal al inicio
        timeRemaining = startTimeInSeconds; // Inicializa el tiempo restante
    }

    void Update()
    {
        if (isTimerRunning)
        {
            // Actualizar el tiempo restante
            timeRemaining -= Time.deltaTime;

            // Cambiar el color del texto a rojo si quedan menos de 10 segundos
            if (timeRemaining <= 10f)
            {
                timerText.color = Color.red; // Cambia el color a rojo
            }


            // Detener el contador cuando llegue a 0
            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                isTimerRunning = false;

                // Notificar que el tiempo se ha agotado
                OnTimerEnd?.Invoke();
                // Ir a la escena de Game Over
                SceneManager.LoadScene("GameOver");
            }

            // Formatear el tiempo en minutos y segundos
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);

            // Mostrar el tiempo en el texto
            timerText.text = $"{minutes:00}:{seconds:00}";
        }

        // Asegurar que el texto siga a la cámara
        UpdateTextPosition();
    }

    private void UpdateTextPosition()
    {
        if (mainCamera != null)
        {
            Vector3 targetPosition = mainCamera.transform.position +
                                     mainCamera.transform.forward * positionOffset.z +
                                     mainCamera.transform.up * positionOffset.y +
                                     mainCamera.transform.right * positionOffset.x;

            timerText.transform.position = targetPosition;
            timerText.transform.rotation = Quaternion.LookRotation(timerText.transform.position - mainCamera.transform.position);
        }
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }
}
