using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyHealth health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        health.OnDeath += Die;
    }

    private void Die(Vector3 position)
    {
        Destroy(gameObject);
    }
}
