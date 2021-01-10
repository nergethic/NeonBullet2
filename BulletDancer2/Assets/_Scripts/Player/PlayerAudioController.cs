using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerAudioController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Player player;
    [SerializeField] AudioSource walkSource;
    [SerializeField] AudioSource dashSource;
    [SerializeField] AudioSource mouthSource;
    [SerializeField] AudioSource hitSource;
    [SerializeField] AudioSource spawnSource;
    [SerializeField] AudioClip[] footstepsClips;
    [SerializeField] AudioClip dash;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip hit;
    [SerializeField] AudioClip spawn;

    void Awake()
    {
        playerController.FootstepEvent += OnFootstep;
        playerController.DashEvent += OnDash;
        player.DeathEvent += OnDeath;
        player.HitEvent += OnHit;
        player.SpawnEvent += OnSpawn;
    }

    private void OnHit()
    {
        hitSource.PlayOneShot(hit);
    }

    private void OnFootstep()
    {

        var rnd = Random.Range(1, footstepsClips.Length);
        var currentClip = footstepsClips[rnd];
        walkSource.PlayOneShot(currentClip);
        footstepsClips[rnd] = footstepsClips[0];
        footstepsClips[0] = currentClip;
    }

    private void OnDash()
    {
        dashSource.PlayOneShot(dash);
    }

    private void OnDeath()
    {
        mouthSource.PlayOneShot(death);
    }

    private void OnSpawn()
    {
        spawnSource.PlayOneShot(spawn);
    }
}
