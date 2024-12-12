using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeG : MonoBehaviour
{
    public Transform player; // El jugador
    public float speed; // Velocidad de movimiento
    public float jumpForce; // Fuerza del salto
    public float detectionRange; // Distancia de detecci�n del jugador
    public float deathForce = 5f; // Fuerza m�nima para "matar" al slime
    public float jumpInterval = 2f; // Intervalo de tiempo entre saltos
    private bool facingRight = false; // Para controlar hacia d�nde est� mirando
    private bool isDead = false;
    private Rigidbody2D rb;
    private float jumpTimer; // Temporizador para los saltos
    public float stopDuration = 2f;
    private bool isStopped = false; // Indica si el slime est� en pausa
    private float stopTimer; // Temporizador para la pausa tras colisi�n
    private Animator animator;
    private bool isGrounded;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.5f;

    // Nuevas variables para controlar las animaciones
    private bool isPreparing = false;
    private bool isJumping = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 
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
            return; // Salir de Update mientras est� en pausa
        }
        // Calcular distancia al jugador
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Si el jugador est� dentro del rango de detecci�n, moverse hacia �l
        if (distanceToPlayer <= detectionRange)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        // Calcular la direcci�n hacia el jugador
        Vector2 direction = (player.position - transform.position).normalized;

        // Aplicar una fuerza en esa direcci�n para moverse hacia el jugador
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

        // Verificar y cambiar la direcci�n de mirada
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
        // Cambiar la direcci�n de la escala en el eje X
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Invierte la escala en el eje X
        transform.localScale = scale;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Colisi�n detectada con el jugador");
            //GameObject.Find("Player").GetComponent<Transform>().position.y;
            float posY = GameObject.Find("Player").GetComponent<Transform>().position.y;
            float posEY = transform.position.y + GameObject.Find("Player").GetComponent<SpriteRenderer>().bounds.size.y / 19;
            if (posY > posEY)
            {
                isDead = true;
                animator.SetBool("isPisado", true); // Activar animaci�n de muerte
                Destroy(gameObject, 0.55f);

            }
            else
            {
                // Si no se cumple la condici�n para morir, el slime entra en pausa
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
        // Aqu� puedes agregar animaci�n de muerte o destrucci�n
        Destroy(gameObject);
    }
}
