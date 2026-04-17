using UnityEngine;

public enum UpgradeType
{
    Damage,
    MoveSpeed,
    AttackSpeed,
    MaxHealth
}

[CreateAssetMenu(menuName = "Upgrades/Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    public UpgradeType type;
    public float value;
}