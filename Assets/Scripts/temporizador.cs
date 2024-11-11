using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Importar para acceder a elementos de UI

public class temporizador : MonoBehaviour
{
     public float levelTime = 60f; // Tiempo límite en segundos
    private float timeRemaining;

    public Text timerText; // Referencia al elemento de texto

    void Start()
    {
        timeRemaining = levelTime;
    }

    void Update()
    {
        // Reducir el tiempo restante
        timeRemaining -= Time.deltaTime;

        // Actualizar el texto del temporizador
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        
        // Comprobar si el tiempo ha llegado a cero
        if (timeRemaining <= 0)
        {
            // Llamar a la función de pérdida
            GameOver();
        }
    }

    void GameOver()
    {
        // Reiniciar el nivel o mostrar pantalla de derrota
        SceneManager.LoadScene("PantallaInicial");
    }
}
