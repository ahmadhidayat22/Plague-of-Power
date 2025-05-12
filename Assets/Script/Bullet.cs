using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        // hancurkan peluru saat kena objek
        Destroy(gameObject);
    }

       // Untuk collider isTrigger = true
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
