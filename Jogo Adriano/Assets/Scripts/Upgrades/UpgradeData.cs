using UnityEngine;

/// <summary>
/// Tipos de upgrades que o jogador pode escolher ao subir de level.
/// </summary>
public enum UpgradeType
{
    Damage,
    MoveSpeed,
    AttackSpeed,
    MaxHealth,
    GrenadeRadius,
    GrenadeDamage
}

/// <summary>
/// Asset configurável de upgrade. Cada arquivo .asset define nome, tipo e valor.
/// </summary>
[CreateAssetMenu(menuName = "Upgrades/Upgrade")]
public class UpgradeData : ScriptableObject
{
    [Header("Exibição")]
    public string upgradeName;

    [Header("Efeito")]
    public UpgradeType type;
    public float value;
}
