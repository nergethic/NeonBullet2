using Assets._Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioController : MonoBehaviour
{
    [SerializeField] Entity enemy;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip hit;
    [SerializeField] AudioClip attack;
    [SerializeField] AudioSource hitSource;
    [SerializeField] AudioSource deathSource;
    [SerializeField] AudioSource attackSource;
    void Awake()
    {
        enemy.DeathEvent += OnDeath;
        enemy.HitEvent += OnHit;
        enemy.AttackEvent += OnAttack;
    }

    private void OnAttack() => attackSource.PlayOneShot(attack);

    private void OnHit() => hitSource.PlayOneShot(hit);

    private void OnDeath()
    {
        gameObject.transform.parent = null;
        deathSource.PlayOneShot(death);
        Destroy(gameObject, death.length);
    }
}
