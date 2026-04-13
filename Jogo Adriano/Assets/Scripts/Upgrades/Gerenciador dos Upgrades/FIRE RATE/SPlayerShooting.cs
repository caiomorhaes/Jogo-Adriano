using UnityEngine;

public class SPlayerShooting : MonoBehaviour
{
    public float baseFireRate = 0.5f; // tempo entre tiros
    private float currentFireRate;

    private float fireRateReduction = 0f;

    private float nextFireTime = 0f;

    public float damage = 10f;

    void Start()
    {
        currentFireRate = baseFireRate;
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + currentFireRate;
        }
    }

    void Shoot()
    {
        Debug.Log("Atirando com intervalo: " + currentFireRate);
        // Aqui entra seu sistema de tiro
    }

    // FIRE RATE UPGRADE
    public void ApplyFireRate(float value)
    {
        fireRateReduction = value;
        currentFireRate = baseFireRate - fireRateReduction;

        if (currentFireRate < 0.05f)
            currentFireRate = 0.05f;

        Debug.Log("Novo FireRate: " + currentFireRate);
    }

    // 💥 DAMAGE UPGRADE
    public void ApplyDamage(float value)
    {
        damage += value;
        Debug.Log("Novo Dano: " + damage);
    }
}