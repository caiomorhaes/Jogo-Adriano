using TMPro;
using UnityEngine;

public class Pontuação : MonoBehaviour
{
    int pontos = 0;
    public TextMeshProUGUI textoPontos;

    float tempo = 0f;
    public TextMeshProUGUI textotempo;

    public TextMeshProUGUI vidaatual;

    public PlayerStats playerStats;

    void Start()
    {
        if (playerStats == null)
        {
            playerStats = FindObjectOfType<PlayerStats>();
        }

        textoPontos.text = "Pontos: " + pontos;
        textotempo.text = "Tempo: 0";
    }

    void Update()
    {
        tempo += Time.deltaTime;
        textotempo.text = "Tempo: " + Mathf.FloorToInt(tempo);

        if (playerStats != null)
        {
            vidaatual.text = "Vida: " + Mathf.FloorToInt(playerStats.currentHealth);
        }
    }

    public void AdicionarPontos(int quantidade)
    {
        pontos += quantidade;
        textoPontos.text = "Pontos: " + pontos;

        Debug.Log("🏆 Pontos: " + pontos);
    }

}