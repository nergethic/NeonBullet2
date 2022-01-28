using System.Collections;
using UnityEngine;

public class Weapon : Item {
    [SerializeField] float speed;
    [SerializeField] new Collider2D collider;
    public ProjectileType projectileType;
    public WeaponType type; 
    public Sprite topView;
    
    Coroutine cor;
    
    public override void Use() {
        base.Use();

        if (playerController.activeWeapon is null) {
            base.Use();
            SetCollider(false);
            playerController.activeWeapon = this;
            playerController.weapon.sprite = topView;
            SetButtonStatus(this, true);
        } else if (playerController.activeWeapon == this) {
            RemoveActiveWeapon();
        } else {
            base.Use();
            playerController.activeWeapon.SetButtonStatus(playerController.activeWeapon, false);
            SetCollider(false);
            playerController.activeWeapon = this;
            playerController.weapon.sprite = topView;
            SetButtonStatus(this, true);
        }
    }

    public void SetCollider(bool enable) {
        collider.enabled = enable;
    }
    
    public void Shoot(Vector2 dir) {
        switch (type) {
            case WeaponType.Basic:
                projectileManager.SpawnProjectile(playerController.transform.position, dir, projectileType, true, speed);
                break;
            
            case WeaponType.Shotgun:
                Vector3 playerPos = playerController.transform.position;
                for (int i = -1; i <= 1; i++) {
                    var bulletDir = projectileManager.GetVectorWithRotation(dir, i * 5f);
                    projectileManager.SpawnProjectile(playerPos, bulletDir, projectileType, true, speed);
                }
                break;
            
            case WeaponType.Rpg:
                var bullet = projectileManager.SpawnProjectile(playerController.transform.position, dir, projectileType, true, speed);
                bullet.gameObject.transform.localScale *= 1.5f;
                break;
            
            case WeaponType.Uzi:
                RestartUziShootCor(dir);
                break;
        }
    }

    void RestartUziShootCor(Vector2 dir) {
        if (cor != null)
            StopCoroutine(cor);
        cor = StartCoroutine(StartShooting(dir));
    }

    public void SetButtonStatus(Weapon weapon, bool isActive) => ItemSlot.SetButtonStatus(this, isActive);

    public void RemoveActiveWeapon() {
        if (playerController.activeWeapon == this) {
            SetCollider(true);
            playerController.activeWeapon = null;
            playerController.weapon.sprite = null;
            SetButtonStatus(this, false);
        }
    }

    IEnumerator StartShooting(Vector2 dir) {
        const int numberOfBalls = 2;
        for (int i = 0; i < numberOfBalls; i++) {
            var bullet = projectileManager.SpawnProjectile(playerController.transform.position, dir, projectileType, true, speed);
            bullet.gameObject.transform.localScale *= 0.6f;
            yield return new WaitForSeconds(0.03f);
        }
    }

    public enum WeaponType {
        Basic,
        Shotgun,
        Rpg,
        Uzi
    }
}

