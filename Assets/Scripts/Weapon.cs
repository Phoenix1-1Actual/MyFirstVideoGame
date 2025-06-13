using System.Collections;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon = false;
    // стрельба:
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    // кол-во пулей за выстрел:
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    // разброс:
    public float spreadIntensity;

    // пуля:
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f; // секунды

    public GameObject muzzleEffect;
    internal Animator animator;
    //перезарядка:
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;
    public enum WeaponModel
    {
      M1911,
      M4_8
    }
    public WeaponModel thisWeaponModel;


    public TextMeshProUGUI ammoDisplay;
    public enum ShootingMode
    {
        Single,
        Burst,
        Auto,
    }

    public ShootingMode currentShootingMode;
    private KeyCode Keymodemouse0;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();
        bulletsLeft = magazineSize;
    }

    void Update()
    {
        

        if (isActiveWeapon)
        {
            GetComponent<Outline>().enabled = false;
            //звук пустого магазина
            if (bulletsLeft == 0 && isShooting)
            {
                SoundManager.Instance.emptyMagazineSoundM1911.Play();
            }
            if (currentShootingMode == ShootingMode.Auto)
            {
                // зажимаем лкм:
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
            {
                // один раз нажимаем лкм:
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }
            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && isReloading == false)
            {
                Reload();
                if (readyToShoot && isShooting == false && isReloading == false && bulletsLeft <= 0)
                {
                    Reload();
                }
            }
            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletsLeft = bulletsPerBurst;
                FireWeapon();
            }
            if (AmmoManager.Instance.ammoDisplay != null)
            {
                AmmoManager.Instance.ammoDisplay.text = $"{bulletsLeft / bulletsPerBurst}/{magazineSize / bulletsPerBurst}";
            }  
        }

    }

    private void FireWeapon()

    {
        bulletsLeft--;
        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("RECOIL");

        SoundManager.Instance.PlayShootingSound(thisWeaponModel);
        readyToShoot = false;
        Vector3 shootingdirection = CalculateDirectionAndSpread().normalized;
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        bullet.transform.forward = shootingdirection;
        // выстреливаем пулю:
        bullet.GetComponent<Rigidbody>().AddForce(shootingdirection * bulletVelocity, ForceMode.Impulse);

        // уничтожаем пулю через некоторое время:
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));
        if (allowReset)
        {
            Invoke("ResetShot",shootingDelay);
            allowReset = false;
        }
        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }
    private void Reload()
    {
        //SoundManager.Instance.reloadingSoundM1911.Play();
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);
        animator.SetTrigger("RELOAD");
        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
    }
    private void ReloadCompleted()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
    }
    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }
    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }
        Vector3 direction = targetPoint - bulletSpawn.position;
        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        return direction + new Vector3(x, y, 0);
    }
    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
