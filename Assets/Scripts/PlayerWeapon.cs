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

    const int maxShotgunMag = 4;
    const int maxShotgunAmmo = 20;
    const int maxSmgMag = 20;
    const int maxSmgAmmo = 120;

    private Weapon currentWeapon;

    private void Start()
    {
        currentWeapon = Weapon.item_pistol;
    }

    public void SwitchWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;

        switch (currentWeapon)
        {
            case Weapon.item_smg:
                playerShoot.ResetReload();
                playerShoot.SetAmmo(maxSmgMag, maxSmgAmmo);
                playerShoot.setTimeBetweenShots(0.1f);
                break;
            case Weapon.item_shotgun:
                playerShoot.ResetReload();
                playerShoot.SetAmmo(maxShotgunMag, maxShotgunAmmo);
                playerShoot.setTimeBetweenShots(0.5f);
                break;
        }
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
