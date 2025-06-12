using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("Arrastra aquí tu jugador (Toly)")]
    public Transform target;  
    
    [Tooltip("Factor de suavidad, entre 0 (sin movimiento) y 1 (sin retardo)")]
    [Range(0f, 1f)]
    public float smooth = 0.125f;  
    
    [Tooltip("Sólo en X: cuánto delante/detrás de la posición del jugador")]
    public Vector3 offset;  

    void Start()
    {
        // Posición inicial de la cámara para evitar ver el fondo vacío
        Vector3 initPos = new Vector3(
            target.position.x + offset.x, 
            transform.position.y, 
            transform.position.z
        );
        transform.position = initPos;
    }

    void LateUpdate()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        // Calcula la X deseada (manteniendo Y y Z de la cámara)
        float desiredX = target.position.x + offset.x;
        Vector3 desiredPosition = new Vector3(desiredX, transform.position.y, transform.position.z);

        // Interpola suavemente entre la posición actual y la deseada
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smooth);
    }
}
