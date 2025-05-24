using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;
    public int damage = 1; // default
    private bool hasHit = false;

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
        if (hasHit) return;

        if (other.CompareTag("Enemy"))
        {
            hasHit = true;
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.canvasHealthBar.SetActive(true);
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject); // atau delay sedikit kalau perlu efek
        }
    }
}
