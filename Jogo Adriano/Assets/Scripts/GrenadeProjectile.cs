using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Projétil da granada: voa até o alvo, explode e aplica dano em área.
/// </summary>
public class GrenadeProjectile : MonoBehaviour
{
    private Transform alvo;
    private float dano;
    private float raio;
    private float velocidade;
    private float tempoDeVida = 3f;
    private bool explodiu;

    /// <summary>
    /// Recebe os valores atuais do player no momento em que a granada é arremessada.
    /// </summary>
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

        // Garante que a granada não fique presa no mapa caso perca o alvo.
        if (tempoDeVida <= 0f || alvo == null)
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

    /// <summary>
    /// Busca inimigos no raio da explosão e garante que cada inimigo receba dano só uma vez.
    /// </summary>
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
        // Mostra o raio da explosão quando a granada está selecionada no editor.
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, raio);
    }
}
