using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float stopDistance = 3f; 
    private Transform player;
   
    private Rigidbody2D rb;
    private Vector2 movement;

    public Animator animator;
    public int maxHealth = 3;

    private int currentHealth;
    float distance;
    [SerializeField] FloatingHealthBar healthBar;
    [SerializeField] Transform canvasHealthBar;
    public float attackCooldown = 2f;
    public int attackDamage = 1;
    private bool canAttack = true;
    private Player playerHealth;


    // loot items;
    [Header("Loot")]
    public List<LootItem> lootTable = new List<LootItem>();


    private void Start()
    {
        
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
       GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        healthBar.UpdateHealthBar(currentHealth, maxHealth);

        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<Player>();
        }
        
       
        if (player == null || playerHealth == null)
        {
            Debug.LogWarning("Player not found!");
        }
    }

    private void Update()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position);
        distance = direction.magnitude;
       
        if ( distance >= stopDistance)
        {
            movement = direction.normalized;
           
            // Optional: Flip arah hadap enemy (menghadap ke player)
            if (direction.x > 0.1f){

                transform.localScale = new Vector3(-2.5f, 2.5f, 2.5f); // Menghadap kanan
                canvasHealthBar.localScale = new Vector3(-1f,1f,1f);
            }
            else if (direction.x < -0.1f){

                transform.localScale = new Vector3(2.5f, 2.5f, 2.5f); // Menghadap kiri
                canvasHealthBar.localScale = new Vector3(1f,1f,1f);
            }

               
        }
        else
        {
            movement = Vector2.zero;
        }
        
        if(distance <= stopDistance && canAttack)
        {
            StartCoroutine(Attack());
            
        }else{
            animator.SetBool("isAttack", false);

        }
        
       

    }

    IEnumerator Attack()
    {
        canAttack = false;
        animator.SetBool("isAttack", true);
      
        Debug.Log("attack player");
         if(playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            Debug.Log("Enemy attacked player! Remaining health: " + playerHealth.currentHealth);
        }
        
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;

    }

    private void FixedUpdate()
    {
        // Debug.Log(movement);
        if (movement != Vector2.zero)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy hit! HP: " + currentHealth);
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        // spawn item yang di drop enemy
        // Tambahkan animasi atau efek di sini jika perlu
        foreach(LootItem lootItem in lootTable)
        {
            if(Random.Range(0f, 100f) < lootItem.dropChance)
            {
                InstantiateLoot(lootItem.itemPrefab);
            }
            // break;
        }
        Destroy(gameObject);
    }

    void InstantiateLoot(GameObject loot)
    {
        if(loot)
        {
            GameObject droppedLoot = Instantiate(loot, transform.position, Quaternion.identity);

            // droppedLoot.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(1); // misalnya bullet memberi 1 damage
            // Destroy(collision.gameObject); // hancurkan peluru setelah kena
        }
        
        
    }


}
