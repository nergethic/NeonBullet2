using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Player player;
    [SerializeField] AudioSource feetSource;
    [SerializeField] AudioSource mouthSource;
    [SerializeField] AudioClip[] footstepsClips;
    [SerializeField] AudioClip dash;
    [SerializeField] AudioClip death;
    // Start is called before the first frame update
    void Start()
    {
        playerController.FootstepEvent += OnFootstep;
        playerController.DashEvent += OnDash;
        player.DeathAction += OnDeath;
    }
    
    private void OnFootstep()
    {
        var rnd = Random.Range(0, footstepsClips.Length);
        feetSource.PlayOneShot(footstepsClips[rnd]);
    }

    private void OnDash()
    {
        feetSource.PlayOneShot(dash);
    }

    private void OnDeath()
    {
        mouthSource.PlayOneShot(death);
    }
}
