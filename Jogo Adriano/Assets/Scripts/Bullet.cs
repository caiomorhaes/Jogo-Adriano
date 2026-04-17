using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f; // Tempo até a bala desaparecer

    void Start()
    {
        Destroy(gameObject, lifeTime); // Destroi depois de um tempo
    }

    void OnCollisionEnter(Collision collision)
    {
        // Destroi a bala ao colidir
        Destroy(gameObject);
    }
}
