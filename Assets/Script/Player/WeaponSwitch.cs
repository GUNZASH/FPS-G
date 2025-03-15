using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public GameObject AK47; // อ้างอิง AK47
    public GameObject Grenade; // อ้างอิงระเบิด
    public AK47 ak47Script; // อ้างอิงสคริปต์ AK47 (ต้องใส่ใน Inspector)

    private int selectedWeapon = 0; // 0 = AK47, 1 = Grenade

    void Start()
    {
        EquipWeapon(selectedWeapon); // เริ่มต้นด้วย AK47
    }

    void Update()
    {
        // กดเลข 1 → AK47
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
            EquipWeapon(selectedWeapon);
        }

        // กดเลข 2 → Grenade
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeapon = 1;
            EquipWeapon(selectedWeapon);
        }

        // ใช้ลูกกลิ้งเมาส์ (Scroll Wheel) สลับอาวุธ
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f || scroll < 0f)
        {
            selectedWeapon = (selectedWeapon == 0) ? 1 : 0; // สลับอาวุธ
            EquipWeapon(selectedWeapon);
        }
    }

    void EquipWeapon(int weaponIndex)
    {
        if (weaponIndex == 0)
        {
            AK47.SetActive(true); // AK47 แสดงตลอด
            Grenade.SetActive(false);
            ak47Script.canShoot = true; // ให้ยิงได้
        }
        else
        {
            AK47.SetActive(true); // AK47 ยังคงแสดง
            Grenade.SetActive(true);
            ak47Script.canShoot = false; // ห้ามยิง AK47
        }
    }
}