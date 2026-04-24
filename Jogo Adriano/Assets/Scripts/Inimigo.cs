using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InimigoVS : MonoBehaviour
{
    public Transform player;
    public float velocidade = 3f;
    public int vida = 2;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 🔧 Configuração recomendada
        rb.useGravity = false; // mantém no plano (top-down)
        rb.constraints = RigidbodyConstraints.FreezeRotation; // evita tombar
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // Direção até o player
        Vector3 direcao = (player.position - transform.position).normalized;

        // Movimento com física (não atravessa parede)
        Vector3 novaPosicao = transform.position + direcao * velocidade * Time.fixedDeltaTime;
        rb.MovePosition(novaPosicao);

        // Rotaciona para olhar o player
        direcao.y = 0f;
        if (direcao != Vector3.zero)
        {
            rb.MoveRotation(Quaternion.LookRotation(direcao));
        }
    }

    // Recebe dano
    public void ReceberDano(int dano)
    {
        vida -= dano;

        Debug.Log(gameObject.name + " tomou dano. Vida atual: " + vida);

        if (vida <= 0)
        {
            Morrer();
        }
    }

    // Morte
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