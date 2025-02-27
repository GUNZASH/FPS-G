using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AK47 : MonoBehaviour
{
    public Camera fpsCamera;
    public Transform gunBarrel; // ตำแหน่งกระสุนออก
    public GameObject bulletImpact; // Effect กระสุนโดน
    public TextMeshProUGUI ammoText; // UI กระสุน

    public float damage = 25f;
    public float fireRate = 0.1f;
    public float recoilAmount = 1f;
    public int maxAmmo = 30;
    public int reserveAmmo = 90;
    private int currentAmmo;
    private bool isReloading = false;

    public float aimFOV = 40f;
    private float normalFOV;
    private bool isAiming = false;

    private float nextTimeToFire = 0f;
    private Vector3 defaultGunPosition;

    void Start()
    {
        currentAmmo = maxAmmo;
        normalFOV = fpsCamera.fieldOfView;
        defaultGunPosition = transform.localPosition;
        UpdateAmmoUI();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire && !isReloading)
        {
            if (currentAmmo > 0)
            {
                Shoot();
                nextTimeToFire = Time.time + fireRate;
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && reserveAmmo > 0)
        {
            StartCoroutine(Reload());
        }

        // Scope
        if (Input.GetMouseButtonDown(1))
        {
            Aim(true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            Aim(false);
        }
    }

    void Shoot()
    {
        currentAmmo--;
        UpdateAmmoUI();

        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        // ยิง Raycast และตรวจสอบว่าชน GameObject ที่มี Collider หรือไม่
        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.collider != null) // ตรวจสอบว่าชน Collider จริง ๆ
            {
                Debug.Log("Hit Collider: " + hit.collider.gameObject.name);

                // สร้าง Bullet Impact ที่ตำแหน่งที่โดน และหมุนไปตามพื้นผิว
                GameObject impact = Instantiate(bulletImpact, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 2f);
            }
        }

        // Recoil Effect
        fpsCamera.transform.localRotation *= Quaternion.Euler(-recoilAmount, Random.Range(-recoilAmount, recoilAmount), 0f);
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(1.5f);

        int ammoToLoad = Mathf.Min(maxAmmo - currentAmmo, reserveAmmo);
        reserveAmmo -= ammoToLoad;
        currentAmmo += ammoToLoad;

        isReloading = false;
        UpdateAmmoUI();
    }

    void Aim(bool isScoping)
    {
        isAiming = isScoping;

        if (isAiming)
        {
            fpsCamera.fieldOfView = aimFOV;
            transform.localPosition = new Vector3(0, 0, 0.3f); // ขยับปืนมาตรงกลาง
        }
        else
        {
            fpsCamera.fieldOfView = normalFOV;
            transform.localPosition = defaultGunPosition;
        }
    }

    void UpdateAmmoUI()
    {
        ammoText.text = currentAmmo + " / " + reserveAmmo;
    }
}
