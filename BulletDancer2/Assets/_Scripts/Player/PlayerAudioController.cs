using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerAudioController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Player player;
    [SerializeField] AudioSource feetSource;
    [SerializeField] AudioSource mouthSource;
    [SerializeField] AudioSource hitSource;
    [SerializeField] AudioSource spawnSource;
    [SerializeField] AudioClip[] footstepsClips;
    [SerializeField] AudioClip dash;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip hit;
    [SerializeField] AudioClip spawn;
    // Start is called before the first frame update
    void Start()
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
        var rnd = Random.Range(0, footstepsClips.Length);
        feetSource.PlayOneShot(footstepsClips[0]);
    }

    private void OnDash()
    {
        feetSource.PlayOneShot(dash);
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
