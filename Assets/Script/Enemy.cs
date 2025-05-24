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
    [Header("Health")]
    public int maxHealth = 10;

    private int currentHealth;

    float distance;
    [SerializeField] FloatingHealthBar healthBar;
    [SerializeField] public GameObject canvasHealthBar;
    public float attackCooldown = 2f;
    public int attackDamage = 6;
    private bool canAttack = true;
    private Player playerHealth;


     private WaveManager waveManager;
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
        canvasHealthBar.SetActive(false);
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        waveManager = FindFirstObjectByType<WaveManager>();
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
                canvasHealthBar.transform.localScale = new Vector3(-1f,1f,1f);
            }
            else if (direction.x < -0.1f){

                transform.localScale = new Vector3(2.5f, 2.5f, 2.5f); // Menghadap kiri
                canvasHealthBar.transform.localScale = new Vector3(1f,1f,1f);
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
            // Debug.Log("Enemy attacked player! Remaining health: " + playerHealth.currentHealth);
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
        Debug.Log("Enemy HP: " + currentHealth);
        currentHealth -= damage;
        Debug.Log("Enemy Hit HP: " + currentHealth);
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        // waveManager.OnEnemyKilled();
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
            // Jarak offset maksimum dari posisi enemy
        float dropRadius = 0.5f; // Bisa diatur sesuai keinginan

        // Offset acak di sekitar enemy (dalam lingkaran)
        Vector2 randomOffset = Random.insideUnitCircle * dropRadius;

        Vector3 dropPosition = transform.position + (Vector3)randomOffset;

        GameObject droppedLoot = Instantiate(loot, dropPosition, Quaternion.identity);

            // droppedLoot.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if (collision.CompareTag("Bullet"))
        // {
        //     Bullet bullet = collision.GetComponent<Bullet>();
        //     if (bullet != null)
        //     {
        //         canvasHealthBar.SetActive(true);
        //         TakeDamage(bullet.damage);
        //         // Destroy(collision.gameObject);
        //     }
        // }
        
        
    }


}
