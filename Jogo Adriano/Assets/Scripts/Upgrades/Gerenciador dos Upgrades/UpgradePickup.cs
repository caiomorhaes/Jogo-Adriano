using UnityEngine;

public class UpgradePickup : MonoBehaviour
{
    public UpgradeData upgradeData;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        UpgradeManager manager = other.GetComponent<UpgradeManager>();

        if (manager == null) return;

        if (manager.ApplyUpgrade(upgradeData))
        {
            Destroy(gameObject);
        }
    }
}