using UnityEngine;

public class InimigoVS : MonoBehaviour
{
    public Transform player;
    public float velocidade = 3f;
    public int vida = 2;
    public int xpDrop = 2;
    public int pontosDrop = 10;

    void Update()
    {
        if (player == null) return;

        // Direção até o player
        Vector3 direcao = (player.position - transform.position).normalized;

        // Move direto até o player
        transform.position += direcao * velocidade * Time.deltaTime;

        // Faz o inimigo olhar pro player (opcional)
        transform.LookAt(player);
    }

    // Método chamado quando o inimigo recebe dano
    public void ReceberDano(int dano)
    {
        vida -= dano;

        Debug.Log(gameObject.name + " tomou dano. Vida atual: " + vida);

        if (vida <= 0)
        {
            Morrer();
        }
    }

    // Destroi o inimigo quando a vida acaba
    void Morrer()
    {
        Debug.Log(gameObject.name + " morreu.");

        // ⭐ XP
        LevelSystem levelSystem = FindObjectOfType<LevelSystem>();
        if (levelSystem != null)
        {
            levelSystem.GanharXP(xpDrop);
        }

        // 🏆 PONTOS (usando seu sistema atual)
        Pontuação pontuacao = FindObjectOfType<Pontuação>();
        if (pontuacao != null)
        {
            pontuacao.AdicionarPontos(pontosDrop);
        }

        Destroy(gameObject);
    }
}