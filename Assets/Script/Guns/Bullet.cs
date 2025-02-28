    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject bulletImpactPrefab; // Effect Bullet Impact
    public float lifetime = 2f; // อายุของกระสุน

    void Start()
    {
        Destroy(gameObject, lifetime); // ทำลายตัวเองหลังจากเวลาผ่านไป
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) return; // ไม่ให้โดนตัวเอง

        // ตรวจสอบว่าชน Environment หรือ Enemy เท่านั้น
        if (other.gameObject.CompareTag("Environment") || other.gameObject.CompareTag("Enemy"))
        {
            // สร้าง Bullet Impact ตรงตำแหน่งที่โดน
            GameObject impact = Instantiate(bulletImpactPrefab, transform.position, Quaternion.LookRotation(-transform.forward));
            Destroy(impact, 2f);

            // ทำลายกระสุน
            Destroy(gameObject);
        }
    }
}