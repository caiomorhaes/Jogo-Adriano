using UnityEngine;

public class Gun : MonoBehaviour
{
    // REFERÊNCIAS
    public GameObject bullet;      // Prefab da bala
    public Transform pontoDisparo; // Local de onde a bala sai
    public Transform player;       // Referência do player

    // CONFIGURAÇÃO
    public float intervaloTiro = 1f;   // Tempo entre tiros
    public float velocidadeBala = 15f; // Velocidade da bala

    private float timer; // Controla o tempo entre disparos

    // LOOP PRINCIPAL
    void Update()
    {
        // Soma o tempo a cada frame
        timer += Time.deltaTime;

        // Procura o inimigo mais próximo
        Transform alvo = BuscarInimigoMaisProximo();

        // Se existir alvo, gira o player para ele
        if (alvo != null)
        {
            GirarParaAlvo(alvo);
        }

        // Se chegou no tempo de atirar
        if (timer >= intervaloTiro)
        {
            // Só atira se tiver alvo
            if (alvo != null)
            {
                Atirar(alvo);
                Debug.Log("Atirou"); // Debug para teste
            }

            // Reseta o timer (IMPORTANTE)
            timer = 0f;
        }
    }

    // BUSCAR INIMIGO MAIS PRÓXIMO
    Transform BuscarInimigoMaisProximo()
    {
        // Pega todos os inimigos com a tag "Inimigo"
        GameObject[] inimigos = GameObject.FindGameObjectsWithTag("Inimigo");

        Transform maisProximo = null;
        float menorDistancia = Mathf.Infinity;

        // Percorre todos os inimigos
        foreach (GameObject inimigo in inimigos)
        {
            // Calcula a distância até o player
            float distancia = Vector3.Distance(player.position, inimigo.transform.position);

            // Guarda o mais próximo
            if (distancia < menorDistancia)
            {
                menorDistancia = distancia;
                maisProximo = inimigo.transform;
            }
        }

        return maisProximo;
    }

    // GIRAR PLAYER PARA O ALVO
    void GirarParaAlvo(Transform alvo)
    {
        // Calcula direção até o inimigo
        Vector3 direcao = alvo.position - player.position;

        // Remove inclinação vertical
        direcao.y = 0f;

        // Evita erro de vetor zero
        if (direcao != Vector3.zero)
        {
            // Rotaciona o player para olhar o inimigo
            player.rotation = Quaternion.LookRotation(direcao);
        }
    }

    // SISTEMA DE TIRO
    void Atirar(Transform alvo)
    {
        // Direção da bala até o inimigo
        Vector3 direcao = alvo.position - pontoDisparo.position;

        // Mantém no plano horizontal
        direcao.y = 0f;

        // Normaliza o vetor
        direcao.Normalize();

        // Instancia a bala
        GameObject balaCriada = Instantiate(
            bullet,
            pontoDisparo.position,
            Quaternion.LookRotation(direcao)
        );

        // Pega o Rigidbody da bala
        Rigidbody rb = balaCriada.GetComponent<Rigidbody>();

        // Aplica velocidade
        if (rb != null)
        {
            rb.linearVelocity = direcao * velocidadeBala;
        }
    }
}