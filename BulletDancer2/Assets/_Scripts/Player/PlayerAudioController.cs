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
    [SerializeField] AudioSource blockSource;
    [SerializeField] AudioClip[] gravelFootsteps;
    [SerializeField] AudioClip[] tileFootsteps;
    [SerializeField] AudioClip dash;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip hit;
    [SerializeField] AudioClip spawn;
    [SerializeField] AudioClip chargeUp;
    [SerializeField] AudioClip shooting;
    [SerializeField] AudioClip pickUp;
    [SerializeField] AudioClip block;

    private AudioClip[] currentFootsteps;

    void Awake()
    {
        currentFootsteps = gravelFootsteps;
        playerController.FootstepEvent += OnFootstep;
        playerController.DashEvent += OnDash;
        playerController.PickUpEvent += OnPickUp;
        playerController.ChargeEvent += OnCharge;
        playerController.StopChargeEvent += OnStopCharge;
        playerController.ShootingEvent += OnShooting;
        player.BlockEvent += OnBlock;
        player.SpawnEvent += OnSpawn;
        player.DeathEvent += OnDeath;
        player.HitEvent += OnHit;
    }

    private void OnBlock()
    {
        blockSource.clip = block;
        blockSource.Play();
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

        var rnd = Random.Range(1, currentFootsteps.Length);
        var currentClip = currentFootsteps[rnd];
        walkSource.PlayOneShot(currentClip);
        currentFootsteps[rnd] = currentFootsteps[0];
        currentFootsteps[0] = currentClip;
    }

    private void OnDash() => dashSource.PlayOneShot(dash);

    private void OnDeath() => mouthSource.PlayOneShot(death);

    private void OnSpawn() => spawnSource.PlayOneShot(spawn);

    public void ChangeFootstepsToGravel() => currentFootsteps = gravelFootsteps;
    public void ChangeFootstepsToTile() => currentFootsteps = tileFootsteps;
}
