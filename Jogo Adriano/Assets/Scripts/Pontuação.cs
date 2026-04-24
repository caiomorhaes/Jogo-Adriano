using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Atualiza os textos e indicadores da HUD principal: pontos, tempo, vida, XP e granada.
/// </summary>
public class Pontuação : MonoBehaviour
{
    [Header("Pontos")]
    public TextMeshProUGUI textoPontos;

    [Header("Tempo")]
    public TextMeshProUGUI textotempo;

    [Header("Vida")]
    public TextMeshProUGUI vidaatual;

    [Header("Referências de gameplay")]
    public PlayerStats playerStats;
    public LevelSystem levelSystem;

    [Header("HUD XP")]
    public Image xpFill;
    public TextMeshProUGUI xpTexto;

    [Header("HUD Granada")]
    public TextMeshProUGUI grenadeStatusTexto;
    public Image grenadeKeyBackground;

    private int pontos = 0;
    private float tempo = 0f;

    void Start()
    {
        if (playerStats == null)
        {
            playerStats = FindFirstObjectByType<PlayerStats>();
        }

        if (levelSystem == null)
        {
            levelSystem = FindFirstObjectByType<LevelSystem>();
        }

        EncontrarReferenciasHudSeNecessario();

        if (textoPontos != null)
        {
            textoPontos.text = "Pontos: " + pontos;
        }

        if (textotempo != null)
        {
            textotempo.text = "Tempo: 0";
        }

        AtualizarHudExtra();
    }

    void Update()
    {
        tempo += Time.deltaTime;

        if (textotempo != null)
        {
            textotempo.text = "Tempo: " + Mathf.FloorToInt(tempo);
        }

        if (playerStats != null && vidaatual != null)
        {
            vidaatual.text = "Vida: " + Mathf.FloorToInt(playerStats.currentHealth);
        }

        AtualizarHudExtra();
    }

    public void AdicionarPontos(int quantidade)
    {
        pontos += quantidade;
        textoPontos.text = "Pontos: " + pontos;

        Debug.Log("🏆 Pontos: " + pontos);
    }


    /// <summary>
    /// Procura objetos reais da HUD na cena caso as referências não tenham sido preenchidas no Inspector.
    /// </summary>
    void EncontrarReferenciasHudSeNecessario()
    {
        if (xpFill == null)
        {
            GameObject fillObject = GameObject.Find("PreenchimentoXP");

            if (fillObject != null)
            {
                xpFill = fillObject.GetComponent<Image>();
            }
        }

        if (xpTexto == null)
        {
            GameObject textoObject = GameObject.Find("TextoXP");

            if (textoObject != null)
            {
                xpTexto = textoObject.GetComponent<TextMeshProUGUI>();
            }
        }

        if (grenadeStatusTexto == null)
        {
            GameObject textoObject = GameObject.Find("TextoGranada");

            if (textoObject != null)
            {
                grenadeStatusTexto = textoObject.GetComponent<TextMeshProUGUI>();
            }
        }

        if (grenadeKeyBackground == null)
        {
            GameObject keyObject = GameObject.Find("TeclaG");

            if (keyObject != null)
            {
                grenadeKeyBackground = keyObject.GetComponent<Image>();
            }
        }
    }

    void AtualizarHudExtra()
    {
        AtualizarBarraXP();
        AtualizarIndicadorGranada();
    }

    void AtualizarBarraXP()
    {
        if (xpFill == null || xpTexto == null || levelSystem == null)
        {
            return;
        }

        int xpNecessario = Mathf.Max(1, levelSystem.xpParaProximoLevel);
        xpFill.fillAmount = Mathf.Clamp01((float)levelSystem.xp / xpNecessario);
        xpTexto.text = "XP " + levelSystem.xp + "/" + xpNecessario;
    }

    void AtualizarIndicadorGranada()
    {
        if (playerStats == null || grenadeStatusTexto == null || grenadeKeyBackground == null)
        {
            return;
        }

        float cooldown = playerStats.GrenadeCooldownRemaining;

        // Durante o cooldown, o texto compacto vira contador em segundos.
        if (cooldown > 0.05f)
        {
            grenadeStatusTexto.text = Mathf.CeilToInt(cooldown) + "s";
            grenadeStatusTexto.color = new Color(0.78f, 0.78f, 0.78f, 1f);
            grenadeKeyBackground.color = new Color(0.18f, 0.18f, 0.18f, 0.72f);
            return;
        }

        grenadeStatusTexto.text = "Granada";
        grenadeStatusTexto.color = Color.white;
        grenadeKeyBackground.color = new Color(0.12f, 0.12f, 0.12f, 0.82f);
    }
}
