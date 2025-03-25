using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : CoreComponent
{
    

    [SerializeField] private GameObject[] deathParticles;


    private ParticleManager ParticleManager { get => particleManager ??= core.GetCoreComponent<ParticleManager>(); }
    private Stats Stats { get => stats ??= core.GetCoreComponent<Stats>(); }


    private Stats stats;
    private ParticleManager particleManager;

    private void OnEnable()
    {
        Stats.OnHealthZero += Die;
    }

    private void OnDisable()
    {
        stats.OnHealthZero -= Die;
    }
    public void Die()
    {
        //foreach (var particle in deathParticles)
        //{
        //    ParticleManager.StartParticles(particle);
        //}
        core.transform.parent.gameObject.SetActive(false);
    }
}
