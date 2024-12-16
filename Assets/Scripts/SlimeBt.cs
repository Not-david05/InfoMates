using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBt : MonoBehaviour
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
            return; // Salir de Update mientras est� en pausa
        }

        // Calcular distancia al jugador
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Si el jugador est� dentro del rango de detecci�n, moverse hacia �l
        if (distanceToPlayer <= detectionRange)
        {
            MoveTowardsPlayer();
            TryJump(); // Intentar saltar mientras se mueve hacia el jugador
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

    private void TryJump()
    {
        // Disminuir el temporizador
        jumpTimer -= Time.deltaTime;

        // Verificar si est� en el suelo antes de intentar saltar
        if (jumpTimer <= 0f && !isPreparing && IsGrounded())
        {
            StartPreparingJump();
        }
    }

    private void StartPreparingJump()
    {
        isPreparing = true; // Activar preparaci�n
        animator.SetBool("isPreparing", true); // Activar animaci�n de preparaci�n
        Invoke(nameof(PerformJump), 0.5f); // Realizar el salto despu�s de un breve retraso
    }

    private void PerformJump()
    {
        if (isDead || isStopped) return;

        // Desactivar animaci�n de preparaci�n
        isPreparing = false;
        animator.SetBool("isPreparing", false);

        // Realizar el salto
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Aplicar fuerza hacia arriba
        jumpTimer = jumpInterval; // Reiniciar el temporizador
        isJumping = true; // Activar estado de salto
        animator.SetBool("isJumping", true); // Activar animaci�n de salto
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
            float playerY = collision.transform.position.y;
            float enemyY = transform.position.y;
            float enemyHeight = GetComponent<SpriteRenderer>().bounds.size.y;

            if (playerY > enemyY + (enemyHeight / 2)) // Compara usando la altura del enemigo
            {
                // El slime muere si el jugador cae desde arriba
                isDead = true;
                animator.SetBool("isPisado", true);
                Destroy(gameObject, 0.55f);
            }
            else
            {
                // Si no cumple, mata al jugador
                FindObjectOfType<Timer>().RestarTimer(5f);
                
                // collision.gameObject.GetComponent<PlayerController>().GameOver();
            }
        }
        // if (isJumping&& IsGrounded())
        // {
        //     // Si el slime toca cualquier superficie, desactivar el estado de salto
        //     isJumping = false;
        //     animator.SetBool("isJumping", false);
        // }

        // if (collision.gameObject.CompareTag("Player"))
        // {
        //     Debug.Log("Colisi�n detectada con el jugador");
        //     //GameObject.Find("Player").GetComponent<Transform>().position.y;
        //     float posY = GameObject.Find("Player").GetComponent<Transform>().position.y;
        //     float posEY = transform.position.y + GameObject.Find("Player").GetComponent<SpriteRenderer>().bounds.size.y / 4;
        //     if (posY > posEY)
        //     {
        //         isDead = true;
        //         animator.SetBool("isPisado", true); // Activar animaci�n de muerte
        //         Destroy(gameObject, 0.58f);

        //     }
        //     else
        //     {
        //         // Si no se cumple la condici�n para morir, el slime entra en pausa
        //         StopMovement();
        //     }
        // }
    }


    private void StopMovement()
    {
        isStopped = true; // Activar la pausa
        stopTimer = stopDuration; // Reiniciar el temporizador de pausa
        rb.velocity = Vector2.zero; // Detener el movimiento del slime
    }
    private bool IsGrounded()
    {
        // Consideramos que el personaje est� en el suelo si la velocidad vertical es casi cero
        bool groundedStatus = Mathf.Abs(rb.velocity.y) < 0.01f;

        // Cambiar el estado de `isJumping` en funci�n de si el personaje est� en el suelo
        if (groundedStatus)
        {
            isJumping = false;
        }

        // Registro de depuraci�n
        Debug.Log("IsGrounded: " + groundedStatus);

        return groundedStatus;
    }
}


