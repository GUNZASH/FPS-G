using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public GameObject gunPrefab;
    public Transform gunHolder;
    public float pickupRange = 2f; // ระยะเก็บปืน

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TryPickupGun();
        }
    }

    void TryPickupGun()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            if (hit.collider.CompareTag("AK47")) // เช็คว่ามีปืนอยู่ตรงกลางจอหรือไม่
            {
                PickupGun(hit.collider.gameObject);
            }
        }
    }

    void PickupGun(GameObject gunObject)
    {
        gunObject.SetActive(false);

        GameObject newGun = Instantiate(gunPrefab, gunHolder.position, gunHolder.rotation, gunHolder);
        newGun.SetActive(true);
    }
}