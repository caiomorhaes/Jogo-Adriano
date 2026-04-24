using UnityEngine;

/// <summary>
/// Arma automática: mira no inimigo mais próximo e dispara no intervalo configurado.
/// </summary>
public class Gun : MonoBehaviour
{
    [Header("Referências")]
    public GameObject bullet;
    public Transform pontoDisparo;
    public Transform player;

    [Header("Tiro")]
    public float intervaloTiro = 1f;
    public float velocidadeBala = 15f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        Transform alvo = BuscarInimigoMaisProximo();

        if (alvo != null)
        {
            GirarParaAlvo(alvo);
        }

        // O tiro só acontece quando o tempo de recarga termina e existe um alvo.
        if (timer >= intervaloTiro)
        {
            if (alvo != null)
            {
                Atirar(alvo);
                Debug.Log("Atirou");
            }

            timer = 0f;
        }
    }

    /// <summary>
    /// Varre todos os objetos marcados como inimigo e retorna o mais perto do player.
    /// </summary>
    Transform BuscarInimigoMaisProximo()
    {
        GameObject[] inimigos = GameObject.FindGameObjectsWithTag("Inimigo");

        Transform maisProximo = null;
        float menorDistancia = Mathf.Infinity;

        foreach (GameObject inimigo in inimigos)
        {
            float distancia = Vector3.Distance(player.position, inimigo.transform.position);

            if (distancia < menorDistancia)
            {
                menorDistancia = distancia;
                maisProximo = inimigo.transform;
            }
        }

        return maisProximo;
    }

    /// <summary>
    /// Gira o player no plano horizontal para olhar na direção do alvo.
    /// </summary>
    void GirarParaAlvo(Transform alvo)
    {
        Vector3 direcao = alvo.position - player.position;
        direcao.y = 0f;

        if (direcao != Vector3.zero)
        {
            player.rotation = Quaternion.LookRotation(direcao);
        }
    }

    /// <summary>
    /// Instancia a bala e empurra o Rigidbody dela na direção do alvo.
    /// </summary>
    void Atirar(Transform alvo)
    {
        Vector3 direcao = alvo.position - pontoDisparo.position;
        direcao.y = 0f;
        direcao.Normalize();

        GameObject balaCriada = Instantiate(
            bullet,
            pontoDisparo.position,
            Quaternion.LookRotation(direcao)
        );

        Rigidbody rb = balaCriada.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = direcao * velocidadeBala;
        }
    }
}
