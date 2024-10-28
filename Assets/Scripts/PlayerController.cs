
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 movement;
    private bool isGrounded;

    

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

        // Detectar si el personaje est� en el suelo usando la funci�n `IsGrounded`
        isGrounded = IsGrounded();
        if (Input.GetKey(KeyCode.A))
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
         else if (Input.GetKey(KeyCode.D))
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // Animaci�n de caminar
        if (moveX != 0)
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isFalling", false);

            // Voltear el sprite dependiendo de la direcci�n del movimiento
           
        }
        else if (!isGrounded)
        {
            animator.SetBool("isFalling", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isFalling", false);
        }

        // Saltar cuando se presiona la tecla "Jump" (por defecto la tecla Espacio)
        
        if (Input. GetKeyDown(KeyCode.Space)) 
        {
            Jump();
        }

        // Actualizar la animaci�n de salto
        animator.SetBool("isJumping", !isGrounded);
    }

    void FixedUpdate()
    {
        // Mover al personaje horizontalmente
        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
    }

    void Jump()
    {
        //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
          rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private bool IsGrounded()
    {
        // Dibuja un peque�o c�rculo en el punto `groundCheck` para verificar colisiones con el suelo
        Collider2D collider = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        return collider != null;
    }


    // M�todo para visualizar el GroundCheck en la ventana de Scene (opcional, �til para depuraci�n)
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}