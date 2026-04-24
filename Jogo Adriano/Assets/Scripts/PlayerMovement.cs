using UnityEngine;

/// <summary>
/// Movimento básico 3D do player usando Rigidbody.
/// </summary>
public class PlayerMovement3D : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 5f;

    private Rigidbody rb;
    private Vector3 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Captura entrada no Update para não perder comandos entre frames de física.
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        movement = new Vector3(x, 0f, z);
    }

    void FixedUpdate()
    {
        // Aplica movimento no Rigidbody e preserva a velocidade vertical da gravidade.
        Vector3 vel = movement * speed;
        vel.y = rb.linearVelocity.y;

        rb.linearVelocity = vel;
    }
}
