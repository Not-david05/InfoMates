using UnityEngine;

public class Star : MonoBehaviour
{
    // Sprite de la estrella
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public float time;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Falta asignar un componente Animator al objeto de la estrella.");
        }
    }

    void Update()
    {
        Timer timer = FindObjectOfType<Timer>(); // Buscar el script Timer din�micamente

        if (timer != null)
        {
            
            float elapsedTime = timer.GetTimeRemaining();

            // Cambiar el estado del animador seg�n el tiempo restante
            if (timer.GetTimeRemaining() <= time)
            {
                animator.SetBool("ON", false);
            }
        }
    }
}


