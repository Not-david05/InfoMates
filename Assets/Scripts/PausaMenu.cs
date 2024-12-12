using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Arrastra aquí el panel principal del menú en el Inspector
    private bool isPaused = false; // Estado del juego

    void Update()
    {
        // Activa o desactiva el menú al pulsar "Esc"
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Oculta el menú
        Time.timeScale = 1f;         // Restaura la velocidad del juego
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true); // Muestra el menú
        Time.timeScale = 0f;         // Detiene el tiempo en el juego
        isPaused = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f; // Asegúrate de restaurar la velocidad del juego antes de reiniciar
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recarga la escena actual
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
         // Solo para depuración en el editor
        Time.timeScale = 1f;
        Application.Quit();  // Cierra el juego al compilar
        SceneManager.LoadScene("Niveles"); 
        
    }
}
