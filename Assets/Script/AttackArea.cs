using UnityEngine;

public class AttackArea : MonoBehaviour
{
    //  [SerializeField] private int damage = 2;
    private Collider2D attackCollider;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("hit", collider);
        // if (collider.CompareTag("Enemy"))
        // {
        //     Enemy enemy = collider.GetComponent<Enemy>();
        //     if (enemy != null)
        //     {
        //         Vector2 hitDirection = (collider.transform.position - transform.position).normalized;
        //         enemy.TakeDamage(damage, hitDirection);
        //     }
        // }
        
    }
}
