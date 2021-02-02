using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioController : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip hit;
    [SerializeField] AudioClip attack;
    [SerializeField] AudioSource mouth;
    [SerializeField] AudioSource attackSource;
    void Awake()
    {
        enemy.DeathEvent += OnDeath;
        enemy.HitEvent += OnHit;
        enemy.AttackEvent += OnAttack;
    }

    private void OnAttack() => attackSource.PlayOneShot(attack);

    private void OnHit() => mouth.PlayOneShot(hit);

    private void OnDeath() => mouth.PlayOneShot(death);

}
