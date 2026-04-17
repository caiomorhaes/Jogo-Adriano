using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;     // Referência ao player
    public Vector3 offset;       // Distância da câmera em relação ao player

    void LateUpdate()
    {
        // Move a câmera mantendo o offset
        transform.position = player.position + offset;
    }
}