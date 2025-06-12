using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damageToPlayer = 1;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Collisiones col = collision.gameObject.GetComponent<Collisiones>();
            if (col != null)
            {
                // Simula daño: fuerza la colisión
                collision.gameObject.SendMessage("OnCollisionEnter2D", collision, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
