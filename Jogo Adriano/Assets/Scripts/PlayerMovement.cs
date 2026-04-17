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
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        movement = new Vector3(x, 0f, z);
    }

    void FixedUpdate()
    {
        Vector3 vel = movement * speed;
        vel.y = rb.linearVelocity.y; // mant�m gravidade

        rb.linearVelocity = vel;
    }
}