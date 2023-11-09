using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;
    public int xpReward = 5;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damage;

        if (currentHealth <=0)
        {
            currentHealth = 0;
            Die();
        }
    }


    public void Heal(int healAmount)
    {
        if (isDead)
        {
            return;
        }

        currentHealth += healAmount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    public void Die()
    {
        isDead = true;

        // Award XP to the player when the enemy is defeated
        PlayerXPManager playerXPManager = FindObjectOfType<PlayerXPManager>();
        if (playerXPManager != null)
        {
            playerXPManager.AddXP(xpReward);
        }

        Destroy(gameObject);
    }
}
