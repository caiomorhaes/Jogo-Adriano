using UnityEngine;

public class InimigoVS : MonoBehaviour
{
    public Transform player;
    public float velocidade = 3f;
    public int vida = 2;
    public int xpDrop = 2;
    public int pontosDrop = 10;

    [Header("Dano no player")]
    public float danoBase = 5f;
    public float multiplicadorDano = 1.5f;
    public float intervaloAumentoDano = 15f;
    public float intervaloEntreDanos = 1f;

    private float proximoDanoPermitido;

    void Update()
    {
        if (player == null) return;

        // Direcao ate o player
        Vector3 direcao = (player.position - transform.position).normalized;

        // Move direto ate o player
        transform.position += direcao * velocidade * Time.deltaTime;

        // Faz o inimigo olhar para o player (opcional)
        transform.LookAt(player);
    }

    void OnCollisionEnter(Collision collision)
    {
        TentarDarDano(collision.gameObject);
    }

    void OnCollisionStay(Collision collision)
    {
        TentarDarDano(collision.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        TentarDarDano(other.gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        TentarDarDano(other.gameObject);
    }

    void TentarDarDano(GameObject alvo)
    {
        if (Time.time < proximoDanoPermitido)
        {
            return;
        }

        PlayerStats playerStats = alvo.GetComponentInParent<PlayerStats>();

        if (playerStats == null)
        {
            return;
        }

        float danoAtual = CalcularDanoAtual();
        playerStats.ReceberDano(danoAtual);
        proximoDanoPermitido = Time.time + intervaloEntreDanos;
    }

    float CalcularDanoAtual()
    {
        if (intervaloAumentoDano <= 0f)
        {
            return danoBase;
        }

        float aumentos = Mathf.Floor(Time.timeSinceLevelLoad / intervaloAumentoDano);
        return danoBase * Mathf.Pow(multiplicadorDano, aumentos);
    }

    // Metodo chamado quando o inimigo recebe dano
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
