using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public UpgradeManager upgradeManager;
    public PlayerStats playerStats;

    public void LevelUp()
    {
        var upgrades = upgradeManager.GetRandomUpgrades(3);

        playerStats.ApplyUpgrade(upgrades[0]);
    }
}