using UnityEngine;

/// <summary>
/// Comportamento principal do inimigo: perseguição, vida escalável e dano por contato.
/// </summary>
public class InimigoVS : MonoBehaviour
{
    [Header("Alvo e movimento")]
    public Transform player;
    public float velocidade = 3f;

    [Header("Vida atual")]
    public int vida = 2;

    [Header("Vida do inimigo")]
    public int vidaBase = 2;
    public float multiplicadorVida = 2f;
    public float intervaloAumentoVida = 60f;

    [Header("Dano no player")]
    public float danoBase = 5f;
    public float multiplicadorDano = 1.5f;
    public float intervaloAumentoDano = 15f;
    public float intervaloEntreDanos = 1f;

    private float proximoDanoPermitido;
    private int nivelVidaAplicado;

    void Start()
    {
        // Inimigos que nascem mais tarde já entram com a vida escalada pelo tempo da partida.
        nivelVidaAplicado = CalcularNivelVidaAtual();
        vida = CalcularVidaPorNivel(nivelVidaAplicado);
    }

    void Update()
    {
        AtualizarVidaPeloTempo();

        if (player == null)
        {
            return;
        }

        // Persegue o player em linha reta.
        Vector3 direcao = (player.position - transform.position).normalized;
        transform.position += direcao * velocidade * Time.deltaTime;

        // Mantém o inimigo visualmente apontado para o player.
        transform.LookAt(player);
    }

    /// <summary>
    /// Dobra/multiplica a vida dos inimigos vivos quando o tempo de dificuldade avança.
    /// </summary>
    void AtualizarVidaPeloTempo()
    {
        int nivelAtual = CalcularNivelVidaAtual();

        if (nivelAtual <= nivelVidaAplicado)
        {
            return;
        }

        float multiplicador = Mathf.Pow(Mathf.Max(1f, multiplicadorVida), nivelAtual - nivelVidaAplicado);
        vida = Mathf.Max(1, Mathf.CeilToInt(vida * multiplicador));
        nivelVidaAplicado = nivelAtual;

        Debug.Log(gameObject.name + " teve a vida aumentada. Vida atual: " + vida);
    }

    int CalcularNivelVidaAtual()
    {
        if (intervaloAumentoVida <= 0f)
        {
            return 0;
        }

        return Mathf.FloorToInt(Time.timeSinceLevelLoad / intervaloAumentoVida);
    }

    int CalcularVidaPorNivel(int nivel)
    {
        float multiplicador = Mathf.Pow(Mathf.Max(1f, multiplicadorVida), nivel);
        return Mathf.Max(1, Mathf.CeilToInt(vidaBase * multiplicador));
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

    /// <summary>
    /// Aplica dano ao player com intervalo, evitando dano a cada frame de contato.
    /// </summary>
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

    /// <summary>
    /// Recebe dano de balas, granadas e outros ataques do player.
    /// </summary>
    public void ReceberDano(int dano)
    {
        vida -= dano;

        Debug.Log(gameObject.name + " tomou dano. Vida atual: " + vida);

        if (vida <= 0)
        {
            Morrer();
        }
    }

    void Morrer()
    {
        Debug.Log(gameObject.name + " morreu.");
        Destroy(gameObject);
    }
}
