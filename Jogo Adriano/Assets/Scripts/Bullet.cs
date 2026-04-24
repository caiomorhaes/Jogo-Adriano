using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 1000f;
    public int dano;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
        }

        PlayerStats player = FindObjectOfType<PlayerStats>();

        if (player != null)
        {
            dano = player.damage;
        }

        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // ignora o player
        if (other.CompareTag("Player"))
        {
            return;
        }

        InimigoVS inimigo = other.GetComponent<InimigoVS>();

        if (inimigo != null)
        {
            inimigo.ReceberDano(dano);
        }

        Destroy(gameObject);
    }
}