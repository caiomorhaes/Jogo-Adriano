using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI[] textos;
    public GameObject hudnormal;

    public void Mostrar(List<UpgradeData> upgrades)
    {
        panel.SetActive(true);
        hudnormal.SetActive(false);

        Debug.Log("Mostrando upgrades: " + upgrades.Count);

        for (int i = 0; i < textos.Length; i++)
        {
            if (i < upgrades.Count && upgrades[i] != null)
            {
                Debug.Log("Upgrade " + i + ": " + upgrades[i].upgradeName);

                textos[i].text =
                    (i + 1) + " - " +
                    upgrades[i].upgradeName +
                    " (+" + upgrades[i].value + ")";
            }
            else
            {
                textos[i].text = "";
            }
        }
    }

    public void Esconder()
    {
        panel.SetActive(false);
        hudnormal.SetActive(true);
    }
}