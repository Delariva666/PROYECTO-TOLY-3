using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animaciones : MonoBehaviour
{
    Animator animator;
    Collisiones collisiones;
    Toly toly;
    Mover mover;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        collisiones = GetComponent<Collisiones>();
        mover = GetComponent<Mover>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Puedes llamar aquí a los métodos según sea necesario
        Grounded();
        VelocityX();
        Jumping();
    }

    public void Grounded()
    {
        animator.SetBool("Grounded", collisiones.Grounded());
    }

    public void VelocityX()
    {
        animator.SetFloat("VelocityX", Mathf.Abs(mover.rb2D.velocity.x));
    }

    public void Jumping()
    {
        animator.SetBool("Jumping", mover.isJumping);
    }

    public void Hurt()
    {
        animator.SetTrigger("Hurt");
    }

    public void Death()
    {
        animator.SetTrigger("Death");
    }
    public void AttackTolyIdle()
    {
        animator.SetTrigger("Attack");
    }
    public void AttackTolyRun()
    {
        animator.SetTrigger("Attack");
    }
}
