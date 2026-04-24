using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public UpgradeManager upgradeManager;
    public PlayerStats playerStats;
    public UpgradeUI upgradeUI; // 🔥 IMPORTANTE

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
        // 🔒 BLOQUEIA TUDO enquanto escolhe upgrade
        if (esperandoEscolha)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) EscolherUpgrade(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) EscolherUpgrade(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) EscolherUpgrade(2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) EscolherUpgrade(3);

            return;
        }

        // ⏱ XP PASSIVO
        timer += Time.deltaTime;

        if (timer >= tempoParaGanharXP)
        {
            timer = 0f;
            GanharXP(xpPorTick);
        }

        // ⭐ LEVEL UP
        if (xp >= xpParaProximoLevel)
        {
            xp -= xpParaProximoLevel;
            level++;

            Debug.Log("🆙 LEVEL UP!");

            esperandoEscolha = true;

            upgradesAtuais = upgradeManager.GetRandomUpgrades(4);

            // 🔥 MOSTRA A HUD
            if (upgradeUI != null)
            {
                upgradeUI.Mostrar(upgradesAtuais);
            }
            else
            {
                Debug.LogError("❌ UpgradeUI NÃO está conectado!");
            }

            Time.timeScale = 0f; // pausa o jogo
        }
    }

    public void GanharXP(int quantidade)
    {
        xp += quantidade;
        Debug.Log("XP: " + xp + "/" + xpParaProximoLevel);
    }

    public void EscolherUpgrade(int index)
    {
        if (upgradesAtuais == null || index >= upgradesAtuais.Count)
        {
            Debug.LogError("❌ Escolha inválida");
            return;
        }

        playerStats.ApplyUpgrade(upgradesAtuais[index]);

        esperandoEscolha = false;
        upgradesAtuais = null;

        // 🔥 ESCONDE HUD
        if (upgradeUI != null)
        {
            upgradeUI.Esconder();
        }

        Time.timeScale = 1f; // volta o jogo
    }
}