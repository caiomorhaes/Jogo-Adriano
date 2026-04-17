using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public List<UpgradeData> allUpgrades;

    public List<UpgradeData> GetRandomUpgrades(int amount)
    {
        List<UpgradeData> selected = new List<UpgradeData>();

        for (int i = 0; i < amount; i++)
        {
            UpgradeData randomUpgrade = allUpgrades[Random.Range(0, allUpgrades.Count)];

            if (!selected.Contains(randomUpgrade))
            {
                selected.Add(randomUpgrade);
            }
            else
            {
                i--; // tenta outro
            }
        }

        return selected;
    }
}