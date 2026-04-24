using UnityEngine;

/// <summary>
/// Mantém a câmera acompanhando o player com uma distância fixa.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Alvo")]
    public Transform player;

    [Header("Enquadramento")]
    public Vector3 offset;

    void LateUpdate()
    {
        // LateUpdate roda depois do movimento do player, evitando tremidas na câmera.
        transform.position = player.position + offset;
    }
}
