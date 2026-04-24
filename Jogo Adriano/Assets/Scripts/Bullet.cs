using UnityEngine;

/// <summary>
/// Controla o ciclo de vida da bala e aplica dano ao inimigo atingido.
/// </summary>
public class Bullet : MonoBehaviour
{
    [Header("Configuração da bala")]
    public float lifeTime = 1000f;
    public int dano;

    void Start()
    {
        // A bala copia o dano atual do player no momento em que é criada.
        PlayerStats player = FindObjectOfType<PlayerStats>();

        if (player != null)
        {
            dano = player.damage;
        }

        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Evita que a própria bala machuque ou destrua no player.
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        // Se o objeto atingido for um inimigo, aplica o dano configurado.
        InimigoVS inimigo = collision.gameObject.GetComponent<InimigoVS>();

        if (inimigo != null)
        {
            inimigo.ReceberDano(dano);
        }

        Destroy(gameObject);
    }
}
