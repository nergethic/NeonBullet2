using UnityEngine;
using UnityEngine.VFX;

public class Portal : MonoBehaviour {
    [SerializeField] VisualEffect vfx;
    [SerializeField] float detectionRadius = 3f;
    
    const string playerTagName = "Player";
    PlayerController player;
    bool isPlaying;

    void Awake() {
        vfx.enabled = true;
        vfx.Stop();
        isPlaying = false;
    }

    void Update() {
        if (player == null) {
            player = FindObjectOfType<PlayerController>(); // TODO: create vfx system that has a ref to player
            if (player == null)
                return;
        }
        
        var diff = player.transform.position - transform.position;
        if (isPlaying) {
            if (diff.sqrMagnitude > detectionRadius + 0.1f) {
                vfx.Stop();
                isPlaying = false;
            }
        } else {
            if (diff.sqrMagnitude < detectionRadius) {
                vfx.Play();
                isPlaying = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag(playerTagName))
            collision.gameObject.transform.position = Vector2.zero;
    }
}
