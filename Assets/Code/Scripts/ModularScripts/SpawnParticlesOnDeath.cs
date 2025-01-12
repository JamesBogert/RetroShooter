using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IDamageable))]
public class SpawnParticlesOnDeath : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> deathParticles = new List<ParticleSystem>();
    [SerializeField] private List<Vector3> offsetList = new List<Vector3>();
    [HideInInspector]
    public IDamageable damageable;

    private void Awake()
    {
        damageable = GetComponent<IDamageable>();
    }

    private void OnEnable()
    {
        damageable.OnDeath += Damageable_OnDeath;
    }

    private void OnDisable()
    {
        damageable.OnDeath -= Damageable_OnDeath;
    }

    private void Damageable_OnDeath(Vector3 position)
    {
        int i = 0;
        foreach (ParticleSystem particle in deathParticles)
        {
            var p = Instantiate(particle, position + offsetList[i], Quaternion.identity);
            Destroy(p, p.main.duration);
            i++;
        }
    }
}
