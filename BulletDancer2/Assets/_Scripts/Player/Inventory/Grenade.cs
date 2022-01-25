using Assets._Scripts.Player.Inventory;
using UnityEngine;

public class Grenade : Item, ThrowableItem {
    [SerializeField] Transform myTransform;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] Rigidbody2D rb;
    Projectile projectile;
    
    public void Throw(float speed, Vector2 itemDirection, Vector2 throwableSpawn) {

        projectile = projectileManager.SpawnProjectile(playerController.transform.position, itemDirection, ProjectileType.Grenade, true, speed);
        if (projectile != null) {
            projectile.DestroyEvent += OnProjectileDestroy;
            ItemSlot.RemoveItemFromSlotOnItemUse();
        }
    }

    void OnProjectileDestroy() {
        var explosion = Instantiate(explosionEffect);
        explosion.transform.position = projectile.transform.position;
        explosion.transform.localScale = new Vector3(1, 1);
        Destroy(explosion, explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }

    public override void Use() {
        if (playerController.ThrowableItem is null) {
            base.Use();
            playerController.ThrowableItem = this;
            SetButtonStatus(this, true);
        } else if (playerController.ThrowableItem is Grenade grenade && grenade == this) {
            RemoveActiveGrenade();
        } else {
            base.Use();
            playerController.ThrowableItem.SetButtonStatus(playerController.ThrowableItem, false);
            playerController.ThrowableItem = this;
            SetButtonStatus(this, true);
        }
    }

    public void RemoveActiveGrenade() {
        if (playerController.ThrowableItem is Grenade grenade && grenade == this) {
            playerController.ThrowableItem = null;
            SetButtonStatus(this, false);
        }
    }

    public void SetButtonStatus(ThrowableItem throwableItem, bool isActive) => ItemSlot.SetButtonStatus(this, isActive);
}
