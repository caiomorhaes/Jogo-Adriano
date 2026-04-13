using UnityEngine;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    //Aqui tem que sincronizar com o comando de quando o jogador atirar, só alterar as variaveis que precisarem ser alteradas, como fireRate, damage, etc
    public SPlayerShooting playerShooting;
    public List<UpgradeState> upgrades;
    public SPlayerShooting playerShooting;
    public List<UpgradeState> upgrades;

    public bool ApplyUpgrade(UpgradeData data)

    public bool ApplyUpgrade(UpgradeData data)
    {
        foreach (var up in upgrades)
        {
            if (up.data == data)
            {
                if (up.IsMaxLevel())
                {
                    Debug.Log("Upgrade no máximo!");
                    return false;
                }

                up.LevelUp();
                ApplyEffect(up);
                return true;
            }
        }

        Debug.LogWarning("Upgrade não encontrado no player!");
        using UnityEngine;
        using System.Collections.Generic;

    {
        {
            foreach (var up in upgrades)
            {
                if (up.data == data)
                {
                    if (up.IsMaxLevel())
                    {
                        Debug.Log("Upgrade no máximo!");
                        return false;
                    }

                    up.LevelUp();
                    ApplyEffect(up);
                    return true;
                }
            }

            Debug.LogWarning("Upgrade não encontrado no player!");
            return false;
        }

            void ApplyEffect(UpgradeState upgrade)
            {
                switch (upgrade.data.type)
                {
                    case UpgradeType.FireRate:
                        playerShooting.ApplyFireRate(upgrade.GetValue());
                        break;

                    case UpgradeType.Damage:
                        playerShooting.ApplyDamage(upgrade.GetValue());
                        break;
