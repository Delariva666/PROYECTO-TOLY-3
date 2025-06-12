using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troler : MonoBehaviour
{
    public float attackRange = 2f; // Rango para detectar al jugador
    public float cooldownTime = 5f; // Tiempo entre ataques
    public int damage = 10; // Daño que inflige al jugador

    private Transform player; // Referencia al jugador
    private bool canAttack = true; // Si puede atacar o no
    private Animator animator; // Para gestionar las animaciones
    public GameObject attackHitbox; // Referencia al GameObject del BoxCollider2D

    private void Start()
    {
        // Encuentra al jugador por su etiqueta (asegúrate de asignarla al jugador)
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();

        // Asegúrate de que el AttackHitbox está desactivado al inicio
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
        }
    }

    private void Update()
    {
        if (player != null)
        {
            // Calcula la distancia al jugador
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Si está dentro del rango y puede atacar
            if (distanceToPlayer <= attackRange && canAttack)
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        canAttack = false; // Desactiva la capacidad de atacar
        animator.SetTrigger("AproximateAttack"); // Activa la animación de ataque

        // Activa el hitbox para detectar colisiones
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(true);
        }

        // Desactiva el hitbox después de un tiempo (asumiendo que la animación dura 0.5s)
        StartCoroutine(DeactivateHitbox());

        // Inicia el cooldown
        StartCoroutine(Cooldown());
    }

    private IEnumerator DeactivateHitbox()
    {
        yield return new WaitForSeconds(0.5f); // Ajusta este tiempo al final de la animación de ataque
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(false);
        }
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        canAttack = true; // Reactiva la capacidad de atacar
    }

    private void OnDrawGizmosSelected()
    {
        // Dibuja un círculo para visualizar el rango de ataque en la escena
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
