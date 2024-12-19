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
        Timer timer = FindObjectOfType<Timer>(); // Buscar el script Timer dinámicamente

        if (timer != null)
        {
            Debug.LogError("Timer encontrado");
            float elapsedTime = timer.GetTimeRemaining();

            // Cambiar el estado del animador según el tiempo restante
            if (timer.GetTimeRemaining() <= time)
            {
                animator.SetBool("ON", false);
            }
        }
    }
}


