using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bullet;
    public Transform pontoDisparo;
    public Transform player;

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

        if (timer >= intervaloTiro)
        {
            if (alvo != null)
            {
                Atirar(alvo);
            }

            timer = 0f;
        }
    }

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

    void GirarParaAlvo(Transform alvo)
    {
        Vector3 direcao = alvo.position - player.position;
        direcao.y = 0f;

        if (direcao != Vector3.zero)
        {
            player.rotation = Quaternion.LookRotation(direcao);
        }
    }

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
            rb.isKinematic = false;
            rb.linearVelocity = direcao * velocidadeBala;
        }
    }
}