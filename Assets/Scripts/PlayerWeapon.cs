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
    private Weapon currentWeapon;

    public void SwitchWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;

        switch (currentWeapon)
        {
            case Weapon.item_smg:
                playerShoot.setTimeBetweenShots(0.1f);
                break;
            case Weapon.item_shotgun:
                playerShoot.setTimeBetweenShots(0.5f);
                break;
        }
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
