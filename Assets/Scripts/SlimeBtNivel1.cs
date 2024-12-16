using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SlimeBtNivel1 : MonoBehaviour
{
    public Transform player; // El jugador
    public float speed; // Velocidad de movimiento
    public float jumpForce; // Fuerza del salto
    public float detectionRange; // Distancia de detección del jugador
    public float deathForce = 5f; // Fuerza mínima para "matar" al slime
    public float jumpInterval = 2f; // Intervalo de tiempo entre saltos
    private bool facingRight = false; // Para controlar hacia dónde está mirando
    private bool isDead = false;
    private Rigidbody2D rb;
    private float jumpTimer; // Temporizador para los saltos
    public float stopDuration = 2f;
    private bool isStopped = false; // Indica si el slime está en pausa
    private float stopTimer; // Temporizador para la pausa tras colisión
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

        // Verificar si está en el suelo antes de intentar saltar
        if (jumpTimer <= 0f && !isPreparing && IsGrounded())
        {
            StartPreparingJump();
        }
    }

    private void StartPreparingJump()
    {
        isPreparing = true; // Activar preparación
        animator.SetBool("isPreparing", true); // Activar animación de preparación
        Invoke(nameof(PerformJump), 0.5f); // Realizar el salto después de un breve retraso
    }

    private void PerformJump()
    {
        if (isDead || isStopped) return;

        // Desactivar animación de preparación
        isPreparing = false;
        animator.SetBool("isPreparing", false);

        // Realizar el salto
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Aplicar fuerza hacia arriba
        jumpTimer = jumpInterval; // Reiniciar el temporizador
        isJumping = true; // Activar estado de salto
        animator.SetBool("isJumping", true); // Activar animación de salto
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
        if (isJumping && IsGrounded())
        {
            // Si el slime toca cualquier superficie, desactivar el estado de salto
            isJumping = false;
            animator.SetBool("isJumping", false);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Colisión detectada con el jugador");

            float posY = collision.gameObject.transform.position.y;
            float posEY = transform.position.y + collision.gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2;

            if (posY > posEY)
            {
                isDead = true;
                animator.SetBool("isPisado", true); // Activar animación de muerte
                Destroy(gameObject, 0.58f);
            }
            else
            {
                // Si no se cumple la condición para morir, el slime repele al jugador
                RepelPlayer(collision.gameObject);
            }
        }
    }

    private void RepelPlayer(GameObject player)
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

        if (playerRb != null)
        {
            // Calcular dirección de repulsión
            Vector2 repelDirection = (player.transform.position - transform.position).normalized;

            // Fuerza de repulsión configurable
            float repelForceHorizontal = 300f; // Ajustar fuerza horizontal
            float repelForceVertical = 170f;   // Ajustar fuerza vertical

            // Aplicar fuerza en ejes horizontal y vertical
            Vector2 repelForce = new Vector2(repelDirection.x * repelForceHorizontal, repelForceVertical);
            playerRb.AddForce(repelForce, ForceMode2D.Impulse);

            // Registro para depuración
            Debug.Log("Repel player with force: " + repelForce);
        }

       
    }






   

    private bool IsGrounded()
    {
        // Consideramos que el personaje está en el suelo si la velocidad vertical es casi cero
        bool groundedStatus = Mathf.Abs(rb.velocity.y) < 0.01f;

        // Cambiar el estado de `isJumping` en función de si el personaje está en el suelo
        if (groundedStatus)
        {
            isJumping = false;
        }

        // Registro de depuración
        Debug.Log("IsGrounded: " + groundedStatus);

        return groundedStatus;
    }
}

