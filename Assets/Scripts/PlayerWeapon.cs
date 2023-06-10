using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapon
{
    item_pistol,
    item_shotgun,
    item_smg
}

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private PlayerShoot playerShoot;

    //[SerializeField] private int maxShotgunMag = 4;
    //[SerializeField] private int maxShotgunAmmo = 20;
    //[SerializeField] private int maxSmgMag = 20;
    //[SerializeField] private int maxSmgAmmo = 120;

    [SerializeField] private int damage = 1;
    private Weapon currentWeapon;
    

    private void Start()
    {
        currentWeapon = Weapon.item_pistol;
    }

    #nullable enable
    public void SwitchWeapon(Weapon weaponTag, WeaponItem? weaponInfo)
    {
        currentWeapon = weaponTag;

        if (weaponInfo != null)
        {
            playerShoot.ResetReload();
            playerShoot.SetWeapon(weaponInfo);
            damage = weaponInfo.damage;
        }
        else
        {
            playerShoot.SetWeaponToPistol();
        }
    }

    public int GetDamage()
    {
        return damage;
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
