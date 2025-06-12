using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    enum Direction { Left = -1, None = 0, Right = 1 }
    Direction currentDirection = Direction.None;
    public float speed;
    public float acceleration;
    public float maxVelocity;
    public float friction;
    float currentVelocity = 0f;

    public float jumpForce;
    public float maxJumpingTime = 1f;
    public bool isJumping;
    float jumpTimer = 0;
    float defaultGravity;

    public Rigidbody2D rb2D;
    Collisiones collisiones;

    public bool InputMoveEnable = true;
    Animator animator;
    public GameObject HitBox;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        collisiones = GetComponent<Collisiones>();
    }

    void Start()
    {
        defaultGravity = rb2D.gravityScale;
    }

    void Update()
    {
        // Desactiva el movimiento si la vida llega a 0
        if (collisiones != null && collisiones.maxHealth <= 0)
        {
            InputMoveEnable = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            StartAttack();
            return;
        }

        if (isJumping)
        {
            if (rb2D.velocity.y < 0f)
            {
                rb2D.gravityScale = defaultGravity;
                if (collisiones.Grounded())
                {
                    isJumping = false;
                    jumpTimer = 0;
                }
            }
            else if (rb2D.velocity.y > 0f)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    jumpTimer += Time.deltaTime;
                }
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    if (jumpTimer < maxJumpingTime)
                    {
                        rb2D.gravityScale = defaultGravity * 3f;
                    }
                }
            }
        }

        currentDirection = Direction.None;

        if (InputMoveEnable)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                currentDirection = Direction.Left;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                currentDirection = Direction.Right;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!InputMoveEnable) return;

        currentVelocity = rb2D.velocity.x;
        if (currentDirection > 0)
        {
            if (currentVelocity < 0)
            {
                currentVelocity += (acceleration + friction) * Time.deltaTime;
            }
            else if (currentVelocity < maxVelocity)
            {
                currentVelocity += acceleration * Time.deltaTime;
                transform.localScale = new Vector2(1, 1);
            }
        }
        else if (currentDirection < 0)
        {
            if (currentVelocity > 0)
            {
                currentVelocity -= (acceleration + friction) * Time.deltaTime;
            }
            else if (currentVelocity > -maxVelocity)
            {
                currentVelocity -= acceleration * Time.deltaTime;
                transform.localScale = new Vector2(-1, 1);
            }
        }
        else
        {
            if (currentVelocity > 1f)
            {
                currentVelocity -= friction * Time.deltaTime;
            }
            else if (currentVelocity < -1f)
            {
                currentVelocity += friction * Time.deltaTime;
            }
            else
            {
                currentVelocity = 0;
            }
        }
        Vector2 velocity = new Vector2(currentVelocity, rb2D.velocity.y);
        rb2D.velocity = velocity;
    }

    void Jump()
    {
        if (collisiones.Grounded() && !isJumping)
        {
            isJumping = true;
            Vector2 fuerza = new Vector2(0, jumpForce);
            rb2D.AddForce(fuerza, ForceMode2D.Impulse);
        }
    }

    void StartAttack()
    {
        if (animator != null)
        {
            // Activar la animación de ataque
            if (currentDirection != Direction.None && !isJumping)
            {
                animator.SetTrigger("RunAttack");
            }
            else if (isJumping)
            {
                animator.SetTrigger("Attack");
            }
            else
            {
                animator.SetTrigger("Attack");
            }

            // Activar el HitBox
            HitBox.SetActive(true);

            // Iniciar la coroutine para desactivar el HitBox después de la animación
            StartCoroutine(StopAttackAfterAnimation());
        }
        else
        {
            Debug.LogError("Animator is not assigned or missing!");
        }
    }

    IEnumerator StopAttackAfterAnimation()
    {
        // Duración de la animación de ataque (ajusta según lo que dure la animación)
        float animationDuration = 0.8f;
        yield return new WaitForSeconds(animationDuration);

        // Desactivar el HitBox después de la animación
        HitBox.SetActive(false);

        if (animator != null)
        {
            animator.ResetTrigger("RunAttack");
            animator.ResetTrigger("Attack");

            if (isJumping)
            {
                animator.SetBool("Jumping", true);
            }
        }
    }
}

