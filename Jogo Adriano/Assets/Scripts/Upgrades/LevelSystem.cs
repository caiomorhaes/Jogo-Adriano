using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla XP, level up e a escolha de upgrades pelo jogador.
/// </summary>
public class LevelSystem : MonoBehaviour
{
    [Header("Referências")]
    public UpgradeManager upgradeManager;
    public PlayerStats playerStats;
    public UpgradeUI upgradeUI;

    [Header("XP")]
    public int xp = 0;
    public int xpParaProximoLevel = 10;
    public int level = 1;

    [Header("Ganho de XP por tempo")]
    public float tempoParaGanharXP = 1f;
    public int xpPorTick = 1;

    private float timer = 0f;
    private bool esperandoEscolha = false;
    private List<UpgradeData> upgradesAtuais;

    void Update()
    {
        // Enquanto a tela de upgrade está aberta, só aceita a escolha 1-4.
        if (esperandoEscolha)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) EscolherUpgrade(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) EscolherUpgrade(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) EscolherUpgrade(2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) EscolherUpgrade(3);

            return;
        }

        timer += Time.deltaTime;

        if (timer >= tempoParaGanharXP)
        {
            timer = 0f;
            GanharXP(xpPorTick);
        }

        if (xp >= xpParaProximoLevel)
        {
            SubirDeLevel();
        }
    }

    public void GanharXP(int quantidade)
    {
        xp += quantidade;
        Debug.Log("XP: " + xp + "/" + xpParaProximoLevel);
    }

    /// <summary>
    /// Sorteia upgrades, mostra a tela de escolha e pausa o jogo.
    /// </summary>
    void SubirDeLevel()
    {
        xp -= xpParaProximoLevel;
        level++;

        Debug.Log("LEVEL UP!");

        esperandoEscolha = true;
        upgradesAtuais = upgradeManager.GetRandomUpgrades(4);

        if (upgradeUI != null)
        {
            upgradeUI.Mostrar(upgradesAtuais);
        }
        else
        {
            Debug.LogError("UpgradeUI NÃO está conectado!");
        }

        Time.timeScale = 0f;
    }

    /// <summary>
    /// Aplica o upgrade selecionado pelo jogador e retoma o jogo.
    /// </summary>
    public void EscolherUpgrade(int index)
    {
        if (upgradesAtuais == null || index >= upgradesAtuais.Count)
        {
            Debug.LogError("Escolha inválida");
            return;
        }

        playerStats.ApplyUpgrade(upgradesAtuais[index]);

        esperandoEscolha = false;
        upgradesAtuais = null;

        if (upgradeUI != null)
        {
            upgradeUI.Esconder();
        }

        Time.timeScale = 1f;
    }
}
