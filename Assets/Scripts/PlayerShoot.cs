using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject pistolBullet;
    [SerializeField] private GameObject shotgunBullet;
    [SerializeField] private GameObject smgBullet;

    [SerializeField] private float reloadDelay = 2f;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private int bulletDamage;
    [SerializeField] private int maxAmmoCapacity = 120;
    [SerializeField] private int maxMagazineCapacity = 30;
    [SerializeField] private Transform offset;
    [SerializeField] private PlayerShootLight playerShootLight;
    [SerializeField] private PlayerWeapon playerWeapon;
    [SerializeField] private AudioClip[] shootAudios;
    [SerializeField] private AudioClip reloadAudio;
    [SerializeField] private TextMeshProUGUI text;

    private int currentAmmo;
    private int currentMag;

    private bool _continuous;
    private float _timeLastFire;

    private bool canShoot = true;
    private Coroutine reloadCourotine;

    private void Start()
    {
        currentAmmo = maxAmmoCapacity;
        currentMag = maxMagazineCapacity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_continuous)
        {
            float timeSinceLastFire = Time.time - _timeLastFire;

            if (canShoot)
            {
                if (timeSinceLastFire >= timeBetweenShots)
                {
                    int randomIndex = Random.Range(0, shootAudios.Length);
                    AudioClip randomGunSound = shootAudios[randomIndex];

                    AudioManager.Instance.PlayQuickAudio(randomGunSound);

                    playerShootLight.FlashGun();

                    HandleShoot();

                    _timeLastFire = Time.time;

                }
            } 
        }        
    }

    private void HandleShoot()
    {
        Weapon currentWeapon = playerWeapon.GetCurrentWeapon();

        if (currentWeapon == Weapon.item_pistol)
        {
            FireBullet();
        }
        else
        {
            if (currentMag > 0)
            {
                if (playerWeapon.GetCurrentWeapon() == Weapon.item_shotgun)
                {
                    FireShotgun();
                }
                else
                {
                    FireBullet();
                }

                currentMag--;

                text.SetText($"<b>Ammo:</b> {currentMag}/{currentAmmo}");

                if (currentMag <= 0)
                {
                    Reload();
                }
            } else
            {
                Reload();
            }
        }

    }

    public void ResetReload()
    {
        if (reloadCourotine != null)
        {
            AudioManager.Instance.StopQuickAudio();
            StopCoroutine(reloadCourotine);
            canShoot = true;
        }
    }

    private IEnumerator ReloadCourotine()
    {
        canShoot = false;  // Disable shooting

        // Wait for the reload time
        yield return new WaitForSeconds(reloadDelay);

        if (currentAmmo > 0 && currentMag < maxMagazineCapacity)
        {
            // Calculate the number of rounds to reload
            int roundsToReload = Mathf.Min(maxMagazineCapacity - currentMag, currentAmmo);

            // Deduct ammo from the total and add to the magazine
            currentAmmo -= roundsToReload;
            currentMag += roundsToReload;

            text.SetText($"<b>Ammo:</b> {currentMag}/{currentAmmo}");
        }
        else
        {
            text.SetText("<b>Ammo:</b> Infinite");
            playerWeapon.SwitchWeapon(Weapon.item_pistol, null);
        }

        canShoot = true;  // Enable shooting again
        reloadCourotine = null;
    }

    private void Reload()
    {
        AudioManager.Instance.PlayQuickAudio(reloadAudio);
        reloadCourotine = StartCoroutine(ReloadCourotine());
    }

    public void SetWeapon(WeaponItem newWeaponItem)
    {
        maxMagazineCapacity = currentMag = newWeaponItem.magazineSize;
        maxAmmoCapacity = currentAmmo = newWeaponItem.ammoSize;
        timeBetweenShots = newWeaponItem.timeBetweenShots;
        bulletDamage = newWeaponItem.damage;

        text.SetText($"<b>Ammo:</b> {currentMag}/{currentAmmo}");
    }

    public void SetWeaponToPistol()
    {
        timeBetweenShots = 0.4f;
        bulletDamage = 1;

        text.SetText($"<b>Ammo:</b> Infinite");
    }

    public void FireBullet()
    {
        GameObject bullet = playerWeapon.GetCurrentWeapon() == Weapon.item_pistol ? pistolBullet : smgBullet;

        GameObject newBullet = Instantiate(bullet, offset.position, transform.rotation);
        Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();

        rb.velocity = bulletSpeed * transform.right;
    }

    public int GetDamage()
    {
        return bulletDamage;
    }

    private void FireShotgun()
    {
        GameObject centerBullet = Instantiate(shotgunBullet, offset.position, transform.rotation);
        Rigidbody2D centerBulletRB = centerBullet.GetComponent<Rigidbody2D>();

        float angle = 8f; // Adjust this value to control the angle of the angled bullets

        GameObject angledBullet1 = Instantiate(shotgunBullet, offset.position, transform.rotation * Quaternion.Euler(0f, 0f, angle));
        Rigidbody2D angledBullet1RB = angledBullet1.GetComponent<Rigidbody2D>();

        GameObject angledBullet2 = Instantiate(shotgunBullet, offset.position, transform.rotation * Quaternion.Euler(0f, 0f, -angle));
        Rigidbody2D angledBullet2RB = angledBullet2.GetComponent<Rigidbody2D>();

        centerBulletRB.velocity = bulletSpeed * transform.right;
        angledBullet1RB.velocity = Quaternion.Euler(0f, 0f, angle) * transform.right * bulletSpeed;
        angledBullet2RB.velocity = Quaternion.Euler(0f, 0f, -angle) * transform.right * bulletSpeed;
    }


    public void setTimeBetweenShots(float newTimeBetweenShots)
    {
        timeBetweenShots = newTimeBetweenShots;
    }

    private void OnFire(InputValue inputValue)
    {
        _continuous = inputValue.isPressed;
    } 
}
