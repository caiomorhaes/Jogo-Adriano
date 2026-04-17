using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 1000f; // Tempo até a bala desaparecer
    public int dano = 1;        // Cada bala tira 1 de vida

    void Start()
    {
        // Destroi a bala sozinha depois de alguns segundos
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Ignora colisão com o player
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        // Tenta pegar o script do inimigo no objeto atingido
        InimigoVS inimigo = collision.gameObject.GetComponent<InimigoVS>();

        // Se acertou um inimigo, aplica dano
        if (inimigo != null)
        {
            inimigo.ReceberDano(dano);
        }

        // Destroi a bala ao colidir
        Destroy(gameObject);
    }
}