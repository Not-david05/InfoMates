using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 1000f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.5f;

    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 movement;
    private bool isGrounded;
    private bool isJumping;  // Nueva bandera para controlar el salto

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Obtener la entrada del usuario para moverse (izquierda/derecha)
        float moveX = Input.GetAxisRaw("Horizontal");

        // Establecer el movimiento
        movement = new Vector2(moveX, 0).normalized;

        // Detectar si el personaje está en el suelo usando la función `IsGrounded`
        isGrounded = IsGrounded();

        // Actualizar la dirección del personaje
        if (Input.GetKey(KeyCode.A))
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // Animación de caminar
        if (moveX != 0 && isGrounded && !isJumping)
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("idle", false);
        }
        else if (isGrounded && !isJumping)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("idle", true);
        }

        // Saltar cuando se presiona la tecla "Jump" y el personaje está en el suelo
        if (isGrounded && !isJumping && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // Actualizar la animación de salto
        animator.SetBool("isJumping", isJumping);
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
        bool groundedStatus = Mathf.Abs(rb.velocity.y) < 0.1f;

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
