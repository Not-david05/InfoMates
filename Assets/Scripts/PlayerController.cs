using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.5f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private bool isGrounded;
    private bool isJumping;

    private Vector3 originalScale;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        

        originalScale = transform.localScale;
        
    }

    void Update()
    {
        // Obtener la entrada del usuario para moverse (izquierda/derecha)
        float moveX = Input.GetAxisRaw("Horizontal");

        // Establecer el movimiento
        movement = new Vector2(moveX, 0).normalized;

        // Detectar si el personaje está en el suelo usando la función `IsGrounded`
        isGrounded = IsGrounded();
        transform.localScale = originalScale;
        
        
        // Actualizar la dirección del personaje
        if (Input.GetKey(KeyCode.A))
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.localScale = new Vector3(-1, 1, 1);
            Vector3 scale = transform.localScale;
        scale.x *= -1; // Invierte la escala en el eje X
        }

        // Actualizar animaciones según el estado
        if (isGrounded)
        {
            if (moveX != 0 && !isJumping)
            {
                // Si está en el suelo y moviéndose, activar la animación de correr
               transform.localScale = originalScale;
                animator.SetBool("isRunning", true);
                animator.SetBool("idle", false);
                animator.SetBool("isJumping", false);
                
            }
            else if (!isJumping)
            {
                // Si está en el suelo y no moviéndose, activar la animación de idle
                transform.localScale = originalScale;
                animator.SetBool("isRunning", false);
                animator.SetBool("idle", true);
                animator.SetBool("isJumping", false);
                
            }
        }
        else
        {
            // Si está en el aire, activar la animación de salto y desactivar las otras
            transform.localScale = originalScale;
            animator.SetBool("isJumping", true);
            animator.SetBool("isRunning", false);
            animator.SetBool("idle", false);
        }

        // Saltar cuando se presiona la tecla "Jump" y el personaje está en el suelo
        if (isGrounded && !isJumping && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // Mover al personaje horizontalmente
        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isJumping = true; // Cambiar a verdadero cuando el personaje salta
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
       

        return groundedStatus;
    }

    // Detectar colisiones con enemigos
    
}

