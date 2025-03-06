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
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 1f); // วาดเส้น Raycast

        Vector3 targetPoint;

        LayerMask enemyLayer = LayerMask.GetMask("Enemy"); // เปลี่ยน "Enemy" เป็นชื่อ Layer ของศัตรู

        // ตรวจจับการชน (ยิงไปที่ศัตรูหรือเปล่า)
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemyLayer)) // เพิ่ม Layer Mask
        {
            Debug.Log("โดน: " + hit.collider.gameObject.name); // เช็คว่าโดนอะไร
            targetPoint = hit.point; // จุดที่ Raycast โดน

            // ✅ ตรวจจับว่าโดนศัตรูหรือไม่
            if (hit.collider.TryGetComponent<EnemyHealth>(out EnemyHealth enemy))
            {
                Debug.Log("โดนศัตรู!"); // เช็คว่าโดนศัตรูจริงๆ ไหม
                enemy.TakeDamage(damage);
            }

            // ✅ สร้าง Impact Effect ตรงจุดที่กระสุนโดน
            //Instantiate(bulletImpact, hit.point, Quaternion.LookRotation(hit.normal));
        }
        else
        {
            targetPoint = ray.GetPoint(150f); // เพิ่มระยะทางการยิงที่ไกลขึ้น
        }

        // คำนวณทิศทางกระสุนให้พุ่งไปยังจุดที่ Raycast โดน
        Vector3 direction = (targetPoint - gunBarrel.position).normalized;

        // ✅ Fake Bullet (กระสุนออกมาตรงๆ โดยไม่สนใจอะไร)
        GameObject bullet = Instantiate(bulletPrefab, gunBarrel.position, Quaternion.LookRotation(direction));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = direction * 50f; // ปรับความเร็วกระสุน
        Destroy(bullet, 0.1f); // กระสุนหายไปเร็วๆ เพื่อไม่ให้มีฟิสิกส์จริง

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
    public void ResetAmmo()
    {
        currentAmmo = maxAmmo;
        reserveAmmo = 180; // หรือค่าที่ต้องการให้เป็นค่าเริ่มต้น
        UpdateAmmoUI();
    }
}
