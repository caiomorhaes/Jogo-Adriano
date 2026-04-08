using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Game/Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    public int maxLevel = 4;
    public float[] values;
}