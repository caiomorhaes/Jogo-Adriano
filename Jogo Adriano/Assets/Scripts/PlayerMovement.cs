using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody rb;
    private Vector3 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Input WASD
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        // Movimento no plano X e Z (3D)
        movement = new Vector3(x, 0f, y);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * speed;
    }
}