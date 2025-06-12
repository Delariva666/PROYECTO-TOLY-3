using System.Collections;
using UnityEngine;

public class MuerteCaida : MonoBehaviour
{
    public Animator playerAnimator;
    public float delayBeforeRestart = 1.5f;

    private bool isRestarting = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isRestarting)
        {
            isRestarting = true;
            StartCoroutine(HandlePlayerFall(collision.gameObject));
        }
    }

    private IEnumerator HandlePlayerFall(GameObject player)
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Death");
        }

        var mover = player.GetComponent<Mover>();
        if (mover != null) mover.InputMoveEnable = false;

        yield return new WaitForSeconds(delayBeforeRestart);

        var col = player.GetComponent<Collisiones>();
        var toly = player.GetComponent<Toly>();

        if (col != null)
        {
            // Simulamos daño directo
            var current = col.IsDead ? 0 : 1;
                   col.StartCoroutine("HandleCollision");

            if (!col.IsDead)
            {
                player.transform.position = Vector3.zero;
                if (mover != null) mover.InputMoveEnable = true;
                isRestarting = false;
                yield break;
            }

            if (toly != null)
            {
                toly.Hit(); // Asegura animación de muerte
            }
        }
    }
}
