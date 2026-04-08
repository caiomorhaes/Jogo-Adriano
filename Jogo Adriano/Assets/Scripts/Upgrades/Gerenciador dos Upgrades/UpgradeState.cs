[System.Serializable]
public class UpgradeState
{
    public UpgradeData data;
    public int currentLevel = 0;

    public bool IsMaxLevel()
    {
        return currentLevel >= data.maxLevel;
    }

    public float GetValue()
    {
        if (currentLevel == 0) return 0;
        return data.values[currentLevel - 1];
    }

    public void LevelUp()
    {
        if (!IsMaxLevel())
            currentLevel++;
    }
}