using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    private Transform alvo;
    private float dano;
    private float raio;
    private float velocidade;
    private float tempoDeVida = 3f;
    private bool explodiu;

    public void Iniciar(Transform novoAlvo, float novoDano, float novoRaio, float novaVelocidade)
    {
        alvo = novoAlvo;
        dano = novoDano;
        raio = novoRaio;
        velocidade = novaVelocidade;
        tempoDeVida = 3f;
        explodiu = false;
    }

    void Update()
    {
        if (explodiu)
        {
            return;
        }

        tempoDeVida -= Time.deltaTime;

        if (tempoDeVida <= 0f)
        {
            Explodir();
            return;
        }

        if (alvo == null)
        {
            Explodir();
            return;
        }

        Vector3 destino = alvo.position + Vector3.up * 0.5f;
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidade * Time.deltaTime);
        transform.Rotate(Vector3.right, 720f * Time.deltaTime);

        if (Vector3.Distance(transform.position, destino) <= 0.25f)
        {
            Explodir();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<InimigoVS>() != null)
        {
            Explodir();
        }
    }

    void Explodir()
    {
        if (explodiu)
        {
            return;
        }

        explodiu = true;
        Collider[] colliders = Physics.OverlapSphere(transform.position, raio);
        HashSet<InimigoVS> inimigosAtingidos = new HashSet<InimigoVS>();

        foreach (Collider collider in colliders)
        {
            InimigoVS inimigo = collider.GetComponentInParent<InimigoVS>();

            if (inimigo != null)
            {
                inimigosAtingidos.Add(inimigo);
            }
        }

        int danoInteiro = Mathf.RoundToInt(dano);

        foreach (InimigoVS inimigo in inimigosAtingidos)
        {
            inimigo.ReceberDano(danoInteiro);
        }

        Debug.Log("Granada explodiu. Inimigos atingidos: " + inimigosAtingidos.Count);
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, raio);
    }
}
