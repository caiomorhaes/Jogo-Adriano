using UnityEngine;
using System.Collections.Generic;
using static UpgradeData;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    public float baseHealth = 100f;
    public float baseDamage = 10f;
    public float baseMoveSpeed = 5f;
    public float baseAttackSpeed = 1f;

    [Header("Current Stats")]
    public float currentHealth;

    private float bonusDamage = 0f;
    private float bonusMoveSpeed = 0f;
    private float bonusAttackSpeed = 0f;
    private float bonusMaxHealth = 0f;

    private List<UpgradeData> activeUpgrades = new List<UpgradeData>();

    public void ApplyUpgrade(UpgradeData upgrade)
    {
        activeUpgrades.Add(upgrade);

        switch (upgrade.type)
        {
            case UpgradeType.Damage:
                bonusDamage += upgrade.value;
                break;

            case UpgradeType.MoveSpeed:
                bonusMoveSpeed += upgrade.value;
                break;

            case UpgradeType.AttackSpeed:
                bonusAttackSpeed += upgrade.value;
                break;

            case UpgradeType.MaxHealth:
                bonusMaxHealth += upgrade.value;
                currentHealth += upgrade.value;
                break;
        }

        Debug.Log("Upgrade aplicado: " + upgrade.upgradeName);
    }
}