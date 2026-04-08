using TMPro;
using UnityEngine;

public class Pontuação : MonoBehaviour
{
    int pontos = 0;
    public TextMeshProUGUI textoPontos;

    float tempo = 0f;
    public TextMeshProUGUI textotempo;

    void Start()
    {
        textoPontos.text = "Pontos: " + pontos;

        textotempo.text = "Tempo: " + tempo;
    }

    
    void Update()
    {
        tempo += Time.deltaTime;
        textotempo.text = "Tempo: " + Mathf.FloorToInt(tempo);
    }
}
