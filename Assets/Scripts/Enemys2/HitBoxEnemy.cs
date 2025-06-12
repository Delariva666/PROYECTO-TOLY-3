using UnityEngine;

public class HitBoxEnemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que el jugador tenga la tag "Player"
        {
            Toly player = other.GetComponent<Toly>();
            if (player != null)
            {
                player.Hit(); // Llama al método Hit del jugador
            }
        }
    }
}

