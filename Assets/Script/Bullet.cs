using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;
    public int damage = 1; // default
    private bool hasHit = false;
    [SerializeField] Transform pfDamagePopup;
    private Animator animator;
    private Rigidbody2D rbBullet;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
        animator = GetComponent<Animator>();
        rbBullet = GetComponent<Rigidbody2D>();
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
                bool isCriticalHit = Random.Range(0f, 100f) < 30;
                int damageAmount = Random.Range(damage, damage + 3); // pakai kalau ingin damage yang random

                DamagePopup.Create(pfDamagePopup, enemy.transform.position, damageAmount, isCriticalHit);
                animator.SetTrigger("isHit");
                enemy.canvasHealthBar.SetActive(true);
                enemy.TakeDamage(damageAmount);
                rbBullet.linearVelocity = new Vector2(0,0);
            }

            Destroy(gameObject, 0.1f); // delay 0.2 detik untuk efek        
        }
    }
}
