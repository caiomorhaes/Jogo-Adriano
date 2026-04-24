using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Guarda todos os upgrades possíveis e sorteia opções únicas para o level up.
/// </summary>
public class UpgradeManager : MonoBehaviour
{
    [Header("Lista de upgrades disponíveis")]
    public List<UpgradeData> allUpgrades;

    /// <summary>
    /// Retorna uma lista aleatória sem repetir upgrades na mesma rodada de escolha.
    /// </summary>
    public List<UpgradeData> GetRandomUpgrades(int amount)
    {
        List<UpgradeData> selected = new List<UpgradeData>();

        if (allUpgrades == null || allUpgrades.Count == 0)
        {
            Debug.LogError("Nenhum upgrade cadastrado!");
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
