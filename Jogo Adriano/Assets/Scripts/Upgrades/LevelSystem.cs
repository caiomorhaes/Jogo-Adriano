using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public UpgradeManager upgradeManager;
    public PlayerStats playerStats;

    [Header("XP")]
    public int xp = 0;
    public int xpParaProximoLevel = 10;
    public int level = 1;

    [Header("Ganho de XP por tempo")]
    public float tempoParaGanharXP = 1f;
    public int xpPorTick = 1;

    private float timer = 0f;

    void Update()
    {
        // ⏱ XP PASSIVO (tempo)
        timer += Time.deltaTime;

        if (timer >= tempoParaGanharXP)
        {
            timer = 0f;
            GanharXP(xpPorTick);
        }

        // ⭐ VERIFICA LEVEL UP
        if (xp >= xpParaProximoLevel)
        {
            xp -= xpParaProximoLevel;
            level++;

            Debug.Log("🆙 LEVEL UP! Level: " + level);

            LevelUp();

            // aumenta dificuldade (opcional)
            xpParaProximoLevel += 5;
        }
    }

    // 🔥 FUNÇÃO PRINCIPAL DE XP
    public void GanharXP(int quantidade)
    {
        xp += quantidade;
        Debug.Log("⭐ XP: " + xp + "/" + xpParaProximoLevel);
    }

    // 🎁 APLICA UPGRADE
    public void LevelUp()
    {
        if (upgradeManager == null || playerStats == null)
        {
            Debug.LogError("❌ Referências não ligadas!");
            return;
        }

        var upgrades = upgradeManager.GetRandomUpgrades(3);

        if (upgrades == null || upgrades.Count == 0)
        {
            Debug.LogError("❌ Nenhum upgrade disponível!");
            return;
        }

        playerStats.ApplyUpgrade(upgrades[0]);
    }
}