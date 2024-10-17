using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class boton : MonoBehaviour
{
    public void CambiarEscena (string nombre){
        SceneManager.LoadScene(nombre);
    }
}
