using UnityEngine;

/// <summary>
/// Comportamento principal do inimigo: perseguição, vida escalável e dano por contato.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
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

    private Rigidbody rb;
    private float proximoDanoPermitido;
    private int nivelVidaAplicado;

    private PlayerStats playerStats;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerStats = FindAnyObjectByType<PlayerStats>();

        // Inimigos que nascem mais tarde já entram com vida escalada
        nivelVidaAplicado = CalcularNivelVidaAtual();
        vida = CalcularVidaPorNivel(nivelVidaAplicado);

        // Configuração física
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // Evita deslizar por impulso de colisão
        rb.linearDamping = 999f;
        rb.angularDamping = 999f;
    }

    void FixedUpdate()
    {
        if (player == null)
            return;

        // Atualiza vida escalada pelo tempo
        AtualizarVidaPeloTempo();

        // Remove qualquer impulso de colisão
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Calcula direção até o player
        Vector3 direcao = (player.position - transform.position).normalized;

        // Move o inimigo em direção ao player
        Vector3 novaPosicao = transform.position + direcao * velocidade * Time.fixedDeltaTime;
        rb.MovePosition(novaPosicao);

        // Faz o inimigo olhar para o player
        direcao.y = 0f;

        if (direcao != Vector3.zero)
        {
            rb.MoveRotation(Quaternion.LookRotation(direcao));
        }
    }

    void AtualizarVidaPeloTempo()
    {
        int nivelAtual = CalcularNivelVidaAtual();

        if (nivelAtual <= nivelVidaAplicado)
            return;

        float multiplicador = Mathf.Pow(
            Mathf.Max(1f, multiplicadorVida),
            nivelAtual - nivelVidaAplicado
        );

        vida = Mathf.Max(1, Mathf.CeilToInt(vida * multiplicador));
        nivelVidaAplicado = nivelAtual;

        Debug.Log(gameObject.name + " teve a vida aumentada. Vida atual: " + vida);
    }

    int CalcularNivelVidaAtual()
    {
        if (intervaloAumentoVida <= 0f)
            return 0;

        return Mathf.FloorToInt(Time.timeSinceLevelLoad / intervaloAumentoVida);
    }

    int CalcularVidaPorNivel(int nivel)
    {
        float multiplicador = Mathf.Pow(
            Mathf.Max(1f, multiplicadorVida),
            nivel
        );

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

    void TentarDarDano(GameObject alvo)
    {
        if (Time.time < proximoDanoPermitido)
            return;

        if (alvo.CompareTag("Player"))
        {
            float danoAtual = CalcularDanoAtual();

            Debug.Log("Inimigo causou dano: " + danoAtual);

            if (playerStats != null)
            {
                playerStats.currentHealth -= danoAtual;
                playerStats.ReceberDano(danoAtual);
            }

            proximoDanoPermitido = Time.time + intervaloEntreDanos;
        }
    }

    float CalcularDanoAtual()
    {
        if (intervaloAumentoDano <= 0f)
            return danoBase;

        float aumentos = Mathf.Floor(Time.timeSinceLevelLoad / intervaloAumentoDano);

        return danoBase * Mathf.Pow(multiplicadorDano, aumentos);
    }

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

        // XP
        LevelSystem levelSystem = FindObjectOfType<LevelSystem>();

        if (levelSystem != null)
        {
            levelSystem.GanharXP(2);
        }

        // Pontuação
        Pontuação pontuacao = FindObjectOfType<Pontuação>();

        if (pontuacao != null)
        {
            pontuacao.AdicionarPontos(10);
        }

        Destroy(gameObject);
    }
}