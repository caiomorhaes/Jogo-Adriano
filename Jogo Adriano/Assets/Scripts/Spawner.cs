using UnityEngine;

public class SpawnerInimigos : MonoBehaviour
{
    public GameObject inimigoPrefab;
    public Transform player;

    [Header("Spawn")]
    public float tempoSpawn = 2f;
    public float distanciaMin = 10f;
    public float distanciaMax = 20f;

    [Header("Dificuldade")]
    public float diminuirTempo = 0.05f; // spawn fica mais rápido
    public float tempoMinimo = 0.5f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= tempoSpawn)
        {
            SpawnarInimigo();
            timer = 0f;

            // aumenta dificuldade com o tempo
            if (tempoSpawn > tempoMinimo)
            {
                tempoSpawn -= diminuirTempo;
            }
        }
    }

    void SpawnarInimigo()
    {
        // direção aleatória ao redor do player
        Vector2 direcao = Random.insideUnitCircle.normalized;

        float distancia = Random.Range(distanciaMin, distanciaMax);

        Vector3 posicaoSpawn = player.position + new Vector3(direcao.x, 0, direcao.y) * distancia;

        GameObject inimigo = Instantiate(inimigoPrefab, posicaoSpawn, Quaternion.identity);

        // conecta o player no inimigo
        inimigo.GetComponent<InimigoVS>().player = player;
    }
}
