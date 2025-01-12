using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int _maxHealth = 100;
    private int _health;

    [SerializeField] private ParticleSystem hitParticle;
    public int currentHealth { get => _health; private set => _health = value; }

    public int maxHealth { get => _maxHealth; private set => _maxHealth = value; }

    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public UnityEvent OnTakeDamageEvent;
    public event IDamageable.DeathEvent OnDeath;
    public UnityEvent OnDeathEvent;

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        int damageTaken = Mathf.Clamp(damage, 0, currentHealth);
        currentHealth -= damageTaken;

        if (damageTaken != 0)
        { 
            OnTakeDamage?.Invoke(damageTaken);
            OnTakeDamageEvent?.Invoke();
        }

        if (currentHealth == 0 && damageTaken != 0)
        {
            OnDeath?.Invoke(transform.position);
            OnDeathEvent?.Invoke();
        }
    }

    public void Impact(Vector3 position, Vector3 normal)
    {
        var hit = Instantiate(hitParticle, position, Quaternion.LookRotation(normal, Vector3.up));
        Destroy(hit, hit.main.duration);
    }
}
