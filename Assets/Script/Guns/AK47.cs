using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AK47 : MonoBehaviour
{
    public Camera fpsCamera;
    public Transform gunBarrel; // ตำแหน่งกระสุนออก
    public GameObject bulletPrefab; // Prefab กระสุน
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

    //ตัวแปร Recoil
    private Vector2 currentRecoil = Vector2.zero;
    private Vector2 recoilSmoothDamp = Vector2.zero;
    public float recoilResetSpeed = 5f;

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
        // ✅ คืนค่า Recoil ค่อยๆ กลับที่เดิม
        currentRecoil = Vector2.SmoothDamp(currentRecoil, Vector2.zero, ref recoilSmoothDamp, recoilResetSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        currentAmmo--;
        UpdateAmmoUI();

        // Raycast จากกลางจอ (fpsCamera) ไปข้างหน้า
        Ray ray = fpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point; // จุดที่ Raycast โดน
        }
        else
        {
            targetPoint = ray.GetPoint(100f); // ถ้าไม่โดนอะไร ยิงไปไกลๆ
        }

        // คำนวณทิศทางกระสุนให้พุ่งไปยังจุดที่ Raycast โดน
        Vector3 direction = (targetPoint - gunBarrel.position).normalized;

        // สร้างกระสุนที่ปลายกระบอกปืน และให้มันพุ่งไปตามทิศทางที่คำนวณ
        GameObject bullet = Instantiate(bulletPrefab, gunBarrel.position, Quaternion.LookRotation(direction));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = direction * 50f; // ปรับความเร็วกระสุน

        // ✅ เพิ่มค่า Recoil สะสม
        currentRecoil += new Vector2(Random.Range(-recoilAmount, recoilAmount), recoilAmount);

        // ✅ ใช้ Recoil Effect
        ApplyRecoil();
    }
    void ApplyRecoil()
    {
        fpsCamera.transform.localRotation *= Quaternion.Euler(-currentRecoil.y, currentRecoil.x, 0f);
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
