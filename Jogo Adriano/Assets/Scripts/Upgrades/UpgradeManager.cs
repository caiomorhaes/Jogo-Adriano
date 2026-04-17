using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public List<UpgradeData> allUpgrades;

    public List<UpgradeData> GetRandomUpgrades(int amount)
    {
        List<UpgradeData> selected = new List<UpgradeData>();

        if (allUpgrades == null || allUpgrades.Count == 0)
        {
            Debug.LogError("❌ Nenhum upgrade cadastrado!");
            return selected;
        }

        amount = Mathf.Min(amount, allUpgrades.Count);

        List<UpgradeData> copy = new List<UpgradeData>(allUpgrades);

        for (int i = 0; i < amount; i++)
        {
            int index = Random.Range(0, copy.Count);
            selected.Add(copy[index]);
            copy.RemoveAt(index);
        }

        return selected;
    }
}