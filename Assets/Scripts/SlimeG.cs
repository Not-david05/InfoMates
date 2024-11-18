using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeG : MonoBehaviour
{
    public Transform player; // El jugador
    public float speed = 2f; // Velocidad de movimiento
    public float detectionRange = 5f; // Distancia de detección del jugador
    public float deathForce = 5f; // Fuerza mínima para "matar" al slime
    private bool facingRight = false; // Para controlar hacia dónde está mirando
    private bool isDead = false;
    private Rigidbody2D rb;
    public float stopDuration = 2f;
    private bool isStopped = false; // Indica si el slime está en pausa
    private float stopTimer; // Temporizador para la pausa tras colisión
    private Animator animator;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isDead) return;
        if (isStopped)
        {
            stopTimer -= Time.deltaTime;
            if (stopTimer <= 0f)
            {
                isStopped = false; // Termina la pausa y el slime puede moverse de nuevo
            }
            return; // Salir de Update mientras está en pausa
        }
        // Calcular distancia al jugador
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Si el jugador está dentro del rango de detección, moverse hacia él
        if (distanceToPlayer <= detectionRange)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        // Calcular la dirección hacia el jugador
        Vector2 direction = (player.position - transform.position).normalized;

        // Aplicar una fuerza en esa dirección para moverse hacia el jugador
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

        // Verificar y cambiar la dirección de mirada
        if (direction.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (direction.x < 0 && facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        // Cambiar la dirección de la escala en el eje X
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Invierte la escala en el eje X
        transform.localScale = scale;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colisión detectada con: " + collision.gameObject.name); // Muestra el nombre del objeto con el que colisiona

        if (collision.gameObject.CompareTag("Player"))
        // collision.gameObject.name ==  collision.gameObject.Player
        {
            Debug.Log("Colisión con el jugador");

            // Si el jugador viene desde arriba con suficiente velocidad, el slime muere
            if (collision.relativeVelocity.y >= deathForce)
            {
                Debug.Log("Jugador aplasta al slime");
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Jugador colisiona con el slime, pero no lo aplasta");
                StopMovement();
            }
        }
    }
    private void StopMovement()
    {
        isStopped = true; // Activar la pausa
        stopTimer = stopDuration; // Reiniciar el temporizador de pausa
        rb.velocity = Vector2.zero; // Detener el movimiento del slime
    }

    private void Die()
    {
        if (isDead) return; // Evitar múltiples llamadas

        isDead = true;
        rb.velocity = Vector2.zero; // Detener el movimiento inmediatamente
        rb.bodyType = RigidbodyType2D.Static; // Detener toda interacción física
        animator.SetBool("isPisado", true); // Activar animación de muerte
        GetComponent<Collider2D>().enabled = false; // Desactivar colisión
        Destroy(gameObject, 0.5f); // Destruir el slime después de la animación
    }
}
