using UnityEngine;

/// <summary>
/// Spawner responsável por criar inimigos ao redor do player e acelerar o ritmo com o tempo.
/// </summary>
public class SpawnerInimigos : MonoBehaviour
{
    [Header("Referências")]
    public GameObject inimigoPrefab;
    public Transform player;

    [Header("Spawn")]
    public float tempoSpawn = 2f;
    public float distanciaMin = 10f;
    public float distanciaMax = 20f;

    [Header("Dificuldade")]
    public float diminuirTempo = 0.05f;
    public float tempoMinimo = 0.5f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= tempoSpawn)
        {
            SpawnarInimigo();
            timer = 0f;

            // A cada spawn, reduz levemente o intervalo até chegar no limite mínimo.
            if (tempoSpawn > tempoMinimo)
            {
                tempoSpawn -= diminuirTempo;
            }
        }
    }

    /// <summary>
    /// Escolhe um ponto aleatório ao redor do player e instancia um inimigo ali.
    /// </summary>
    void SpawnarInimigo()
    {
        Vector2 direcao = Random.insideUnitCircle.normalized;
        float distancia = Random.Range(distanciaMin, distanciaMax);
        Vector3 posicaoSpawn = player.position + new Vector3(direcao.x, 0, direcao.y) * distancia;

        GameObject inimigo = Instantiate(inimigoPrefab, posicaoSpawn, Quaternion.identity);

        // Conecta o player ao inimigo recém-criado para ele saber quem perseguir.
        inimigo.GetComponent<InimigoVS>().player = player;
    }
}
