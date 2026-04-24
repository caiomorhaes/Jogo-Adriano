using UnityEngine;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    public float baseHealth = 100f;
    public float currentHealth;

    private PlayerMovement3D movement;

    private List<UpgradeData> activeUpgrades = new List<UpgradeData>();

    public int damage = 1;

    private Gun gun;

    void Start()
    {
        currentHealth = baseHealth;
        movement = GetComponent<PlayerMovement3D>();
        gun = FindAnyObjectByType<Gun>();
    }

    public void ApplyUpgrade(UpgradeData upgrade)
    {
        if (upgrade == null)
        {
            Debug.LogError("❌ Upgrade é NULL!");
            return;
        }

        activeUpgrades.Add(upgrade);

        switch (upgrade.type)
        {
            case UpgradeType.Damage:
                damage += (int)upgrade.value;
                Debug.Log("💥 Dano: " + damage);
                break;
            case UpgradeType.MoveSpeed:
                if (movement != null)
                {
                    movement.speed += upgrade.value;
                    movement.speed = Mathf.Clamp(movement.speed, 0f, 20f);
                    Debug.Log("🏃 Velocidade: " + movement.speed);
                }
                break;

            case UpgradeType.AttackSpeed:
                Debug.Log("⚡ Aplicando upgrade de velocidade de ataque...");
                if (gun != null)
                {
                    Debug.Log("achei a arma");
                    gun.intervaloTiro -= 0.1f;

                    // 🔥 evita ficar absurdo (importante)
                    gun.intervaloTiro = Mathf.Clamp(gun.intervaloTiro, 0.1f, 10f);

                    Debug.Log("⚡ Intervalo de tiro: " + gun.intervaloTiro);
                }
                break;

            case UpgradeType.MaxHealth:
                currentHealth += upgrade.value;
                Debug.Log("❤️ Vida: " + currentHealth);
                break;
        }

        Debug.Log("✅ Upgrade aplicado: " + upgrade.upgradeName);
    }
}