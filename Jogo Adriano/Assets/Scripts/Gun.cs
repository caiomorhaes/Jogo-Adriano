using UnityEngine;

public class Gun : MonoBehaviour
{
    public float lifeTime = 3f; // Tempo até a bala desaparecer

    void Start()
    {
        Destroy(gameObject, lifeTime); // Destroi depois de um tempo
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroi a bala ao colidir
        Destroy(gameObject);
    }
}