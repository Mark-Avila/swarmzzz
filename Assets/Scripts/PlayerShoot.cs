using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float reloadDelay = 2f;
    [SerializeField] private Transform offset;
    [SerializeField] private PlayerShootLight playerShootLight;
    [SerializeField] private PlayerWeapon playerWeapon;

    [SerializeField] private float bulletSpeed;
    [SerializeField] private float timeBetweenShots;

    [SerializeField] private AudioClip[] shootAudios;
    [SerializeField] private AudioClip reloadAudio;

    [SerializeField] private int maxAmmoCapacity = 120;
    [SerializeField] private int maxMagazineCapacity = 30;

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
    void Update()
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
            } else
            {

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
            playerWeapon.SwitchWeapon(Weapon.item_pistol);
        }

        canShoot = true;  // Enable shooting again
        reloadCourotine = null;
    }
    private void Reload()
    {
        AudioManager.Instance.PlayQuickAudio(reloadAudio);
        reloadCourotine = StartCoroutine(ReloadCourotine());
    }

    public void FireBullet()
    {
        GameObject newBullet = Instantiate(bullet, offset.position, transform.rotation);
        Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();

        rb.velocity = bulletSpeed * transform.right;
    }

    public void SetAmmo(int newMagSize, int newAmmoSize)
    {
        maxMagazineCapacity = currentMag = newMagSize;
        maxAmmoCapacity = currentAmmo = newAmmoSize;

        text.SetText($"<b>Ammo:</b> {currentMag}/{currentAmmo}");
    }

    private void FireShotgun()
    {
        GameObject centerBullet = Instantiate(bullet, offset.position, transform.rotation);
        Rigidbody2D centerBulletRB = centerBullet.GetComponent<Rigidbody2D>();
        centerBulletRB.velocity = bulletSpeed * transform.right;

        float angle = 15f; // Adjust this value to control the angle of the angled bullets

        GameObject angledBullet1 = Instantiate(bullet, offset.position, transform.rotation * Quaternion.Euler(0f, 0f, angle));
        Rigidbody2D angledBullet1RB = angledBullet1.GetComponent<Rigidbody2D>();
        angledBullet1RB.velocity = Quaternion.Euler(0f, 0f, angle) * transform.right * bulletSpeed;

        GameObject angledBullet2 = Instantiate(bullet, offset.position, transform.rotation * Quaternion.Euler(0f, 0f, -angle));
        Rigidbody2D angledBullet2RB = angledBullet2.GetComponent<Rigidbody2D>();
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
