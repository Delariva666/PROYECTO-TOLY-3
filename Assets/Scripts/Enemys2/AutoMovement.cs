using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMovement : MonoBehaviour
{
    public float speed = 1f; // Velocidad del movimiento

    private Rigidbody2D rb2D;
    bool movementePaused;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Establecer velocidad inicial
        rb2D.velocity = new Vector2(speed, rb2D.velocity.y);

        // Asegurarse de que el sprite esté orientado correctamente al inicio
        FlipOnStart();
    }

    private void FixedUpdate()
    {
       
        // Cambiar dirección si el movimiento es muy lento
        if (rb2D.velocity.x > -0.1f && rb2D.velocity.x < 0.1f)
        {
            speed = -speed;
            Flip();
        }

        // Aplicar movimiento
        rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
    }

    private void Flip()
    {
        // Voltear sprite según dirección de movimiento
        Vector3 localScale = transform.localScale;
        localScale.x = speed > 0 ? -Mathf.Abs(localScale.x) : Mathf.Abs(localScale.x);
        transform.localScale = localScale;
    }

    private void FlipOnStart()
    {
        // Ajustar la orientación inicial del sprite
        Vector3 localScale = transform.localScale;
        localScale.x = speed > 0 ? -Mathf.Abs(localScale.x) : Mathf.Abs(localScale.x);
        transform.localScale = localScale;
    }
    public void PauseMovement()
    {
        if (!movementePaused)
        {
            movementePaused = true;
            rb2D.velocity = new Vector2(0, 0);
        }
    }
    
    public void ResumeMovement()
{
    if (movementePaused)
    {
        movementePaused = false;
        rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
    }
}

}
