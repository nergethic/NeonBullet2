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
    [SerializeField] AudioSource gunSource;
    [SerializeField] AudioSource spawnSource;
    [SerializeField] AudioSource pickUpSource;
    [SerializeField] AudioClip[] footstepsClips;
    [SerializeField] AudioClip dash;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip hit;
    [SerializeField] AudioClip spawn;
    [SerializeField] AudioClip chargeUp;
    [SerializeField] AudioClip shooting;
    [SerializeField] AudioClip pickUp;

    void Awake()
    {
        playerController.FootstepEvent += OnFootstep;
        playerController.DashEvent += OnDash;
        playerController.PickUpEvent += OnPickUp;
        playerController.ChargeEvent += OnCharge;
        playerController.StopChargeEvent += OnStopCharge;
        playerController.ShootingEvent += OnShooting;
        player.SpawnEvent += OnSpawn;
        player.DeathEvent += OnDeath;
        player.HitEvent += OnHit;
    }

    private void OnPickUp() => pickUpSource.PlayOneShot(pickUp);

    private void OnShooting()
    {
        gunSource.clip = shooting;
        gunSource.Play();
    }

    private void OnCharge()
    {
        gunSource.clip = chargeUp;
        gunSource.Play();
    }

    private void OnStopCharge() => gunSource.Stop();

    private void OnHit() => hitSource.PlayOneShot(hit);

    private void OnFootstep()
    {

        var rnd = Random.Range(1, footstepsClips.Length);
        var currentClip = footstepsClips[rnd];
        walkSource.PlayOneShot(currentClip);
        footstepsClips[rnd] = footstepsClips[0];
        footstepsClips[0] = currentClip;
    }

    private void OnDash() => dashSource.PlayOneShot(dash);

    private void OnDeath() => mouthSource.PlayOneShot(death);

    private void OnSpawn() => spawnSource.PlayOneShot(spawn);
}
