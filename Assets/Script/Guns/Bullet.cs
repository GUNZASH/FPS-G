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
        if (other.CompareTag("Enemy"))
        {
            // ถ้าใช้ Raycast ยิงอยู่แล้ว ไม่ต้องให้ Bullet ทำดาเมจซ้ำ
            Destroy(gameObject);
        }
    }
}