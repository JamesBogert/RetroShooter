using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private int maxHealth;
    [SerializeField] private PlayerInfoSO playerInfo;

    public GameEvent PlayerDeath;
    public GameEvent PlayerTakeDamage;
    public GameEvent PlayerGivenHealth;

    private void Awake()
    {
        playerInfo.MaxHealth = maxHealth;
        playerInfo.CurrentHealth = maxHealth;
    }

    private void OnDisable()
    {
        playerInfo.MaxHealth = maxHealth;
        playerInfo.CurrentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    { 
        playerInfo.CurrentHealth -= damage;

        if (damage != 0)
        {
            PlayerTakeDamage.Raise();
        }

        if (playerInfo.CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void GiveHealth(int healthGiven)
    { 
        Mathf.Clamp(healthGiven, 0, playerInfo.MaxHealth);
        playerInfo.CurrentHealth += healthGiven;

        if (healthGiven != 0)
        {
            PlayerGivenHealth.Raise();
        }

    }

    void Die()
    {
        PlayerDeath.Raise();
    }
}
