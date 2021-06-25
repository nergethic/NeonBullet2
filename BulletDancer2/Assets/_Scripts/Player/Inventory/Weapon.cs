using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    [SerializeField] float speed;
    public ProjectileType projectileType;
    public WeaponType type; 
    public Sprite topView;
    private Coroutine cor;
    public override void Use()
    {
        base.Use();

        if (playerController.activeWeapon is null)
        {
            base.Use();
            playerController.activeWeapon = this;
            playerController.weapon.sprite = topView;
            SetButtonStatus(this, true);
        }
        else if (playerController.activeWeapon == this)
        {
            RemoveActiveWeapon();
        }
        else
        {
            base.Use();
            playerController.activeWeapon.SetButtonStatus(playerController.activeWeapon, false);
            playerController.activeWeapon = this;
            playerController.weapon.sprite = topView;
            SetButtonStatus(this, true);
        }
    }

    public void RemoveActiveWeapon()
    {
        if (playerController.activeWeapon == this)
        {
            playerController.activeWeapon = null;
            playerController.weapon.sprite = null;
            SetButtonStatus(this, false);
        }
    }

    public void Shoot(Vector2 dir)
    {
        switch (type)
        {
            case WeaponType.Basic:
                projectileManager.SpawnProjectile(playerController.transform.position, dir, projectileType, true, speed);
                break;
            case WeaponType.Shotgun:
                projectileManager.SpawnProjectile(playerController.transform.position, dir, projectileType, true, speed);
                projectileManager.SpawnProjectile(playerController.transform.position, projectileManager.GetVectorWithRotation(dir, 25), projectileType, true, speed);
                projectileManager.SpawnProjectile(playerController.transform.position, projectileManager.GetVectorWithRotation(dir, -25), projectileType, true, speed);
                break;
            case WeaponType.Rpg:
                var bullet = projectileManager.SpawnProjectile(playerController.transform.position, dir, projectileType, true, speed);
                bullet.gameObject.transform.localScale *= 1.5f;
                break;
            case WeaponType.Uzi:
                if (cor != null)
                {
                    StopCoroutine(cor);
                    cor = null;
                }
                cor = StartCoroutine(StartShooting(dir));
                break;
            default:
                break;
        }
    }

    public void SetButtonStatus(Weapon weapon, bool isActive) => ItemSlot.SetButtonStatus(this, isActive);

    IEnumerator StartShooting(Vector2 dir)
    {
        var numberOfBalls = 3;

        for (int i = 0; i < numberOfBalls; i++)
        {
            var bullet = projectileManager.SpawnProjectile(playerController.transform.position, dir, projectileType, true, speed);
            bullet.gameObject.transform.localScale *= 0.6f;
            yield return new WaitForSeconds(0.03f);
        }
    }

    public enum WeaponType
    {
        Basic,
        Shotgun,
        Rpg,
        Uzi
    }
}
