using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 1000f;
    public int dano;

    void Start()
    {
        // Pega o dano do PlayerStats quando a bala nasce
        PlayerStats player = FindObjectOfType<PlayerStats>();

        if (player != null)
        {
            dano = player.damage;
        }

        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        InimigoVS inimigo = collision.gameObject.GetComponent<InimigoVS>();

        if (inimigo != null)
        {
            inimigo.ReceberDano(dano);
        }

        Destroy(gameObject);
    }
}