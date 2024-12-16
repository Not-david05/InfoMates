using UnityEngine;

public class Star : MonoBehaviour
{
    public Sprite[] starSprites; // Lista de sprites de la estrella
    public Timer timer; // Referencia al temporizador
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (timer == null)
        {
            Debug.LogError("Falta asignar el temporizador al script de la estrella.");
        }
    }

    void Update()
    {
        if (timer != null)
        {
            float elapsedTime = timer.GetTimeRemaining();

            // Cambiar el sprite según el tiempo transcurrido
            if (elapsedTime < 10f)
            {
                spriteRenderer.sprite = starSprites[0];
            }
            else if (elapsedTime < 20f)
            {
                spriteRenderer.sprite = starSprites[1];
            }
            else if (elapsedTime < 30f)
            {
                spriteRenderer.sprite = starSprites[2];
            }
            else
            {
                spriteRenderer.sprite = starSprites[3];
            }
        }
    }
}

