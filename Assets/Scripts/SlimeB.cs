using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeB : MonoBehaviour
{
    public Transform player; // El jugador
    public float speed = 2f; // Velocidad de movimiento
    public float jumpForce = 5f; // Fuerza del salto
    public float detectionRange = 5f; // Distancia de detección del jugador
    public float deathForce = 5f; // Fuerza mínima para "matar" al slime
    public float jumpInterval = 2f; // Intervalo de tiempo entre saltos
    private bool facingRight = false; // Para controlar hacia dónde está mirando
    private bool isDead = false;
    private Rigidbody2D rb;
    private float jumpTimer; // Temporizador para los saltos
    public float stopDuration = 2f;
    private bool isStopped = false; // Indica si el slime está en pausa
    private float stopTimer; // Temporizador para la pausa tras colisión

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpTimer = jumpInterval; // Inicializar el temporizador con el intervalo de salto
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
            TryJump(); // Intentar saltar mientras se mueve hacia el jugador
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

    private void TryJump()
    {
        // Disminuir el temporizador
        jumpTimer -= Time.deltaTime;

        // Si el temporizador llega a 0, hacer que el slime salte y restablecer el temporizador
        if (jumpTimer <= 0f)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Aplicar fuerza hacia arriba
            jumpTimer = jumpInterval; // Reiniciar el temporizador
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
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Colisión detectada con el jugador");

            // Si el jugador viene desde arriba y tiene suficiente velocidad, el slime muere
            if (collision.relativeVelocity.y >= deathForce)
            {
                Die();
            }
            else
            {
                // Si no se cumple la condición para morir, el slime entra en pausa
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
        isDead = true;
        // Aquí puedes agregar animación de muerte o destrucción
        Destroy(gameObject);
    }
}

