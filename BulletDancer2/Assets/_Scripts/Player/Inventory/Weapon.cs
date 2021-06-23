using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public ProjectileType projectileType;
    public Sprite topView;
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
        playerController.activeWeapon = null;
        playerController.weapon.sprite = null;
        SetButtonStatus(this, false);
    }

    public void SetButtonStatus(Weapon weapon, bool isActive) => ItemSlot.SetButtonStatus(this, isActive);
}
