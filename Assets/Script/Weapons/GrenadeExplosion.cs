using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{
    public float explosionRadius = 5f; // รัศมีระเบิด
    public float damage = 100f; // ดาเมจของระเบิด
    public GameObject explosionEffect; // เอฟเฟกต์ระเบิด
    public LayerMask collisionLayer; // เลเยอร์ที่ระเบิดสามารถชนได้
    private bool isExploded = false; // ตัวแปรตรวจสอบว่าเกิดการระเบิดแล้วหรือยัง
    private float additionalWaitTime = 0f; // เวลารอเพิ่มเติมจากการ Hold

    // ฟังก์ชัน Activate จะรับพารามิเตอร์ของเวลาที่ผู้เล่น Hold
    public void Activate(float holdTime)
    {
        // คำนวณเวลารอเพิ่มจากการ Hold
        additionalWaitTime = Mathf.Clamp(holdTime / 2f, 0f, 2f); // ตัวอย่าง: คูณให้เวลารอเพิ่มขึ้นเมื่อ Hold

        // เช็คว่าเกิดระเบิดแล้วหรือยัง
        if (!isExploded)
        {
            StartCoroutine(Explode());
            isExploded = true; // ให้แน่ใจว่าระเบิดจะทำงานแค่ครั้งเดียว
        }
    }

    IEnumerator Explode()
    {
        // รอเวลาเล็กน้อยก่อนระเบิด (ขึ้นอยู่กับเวลาที่ Hold)
        yield return new WaitForSeconds(2f + additionalWaitTime);

        // สร้างเอฟเฟกต์ระเบิด
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // ค้นหาศัตรูในรัศมีระเบิด
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, collisionLayer);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.TryGetComponent<EnemyHealth>(out EnemyHealth enemy))
            {
                enemy.TakeDamage(damage); // ส่งความเสียหายไปยังศัตรู
            }
        }

        Destroy(gameObject); // ทำลายระเบิดหลังจากระเบิด
    }

    void OnCollisionEnter(Collision collision)
    {
        // ตรวจสอบว่าเราได้ชนกับพื้นหรือวัตถุที่กำหนดให้สามารถระเบิดได้
        if (collision.relativeVelocity.magnitude > 1f && !isExploded) // แรงกระแทกมีค่าเกิน 1 และยังไม่เกิดระเบิด
        {
            Activate(0f); // เรียกใช้งานการระเบิดทันทีเมื่อชนพื้นหรือสิ่งกีดขวาง
        }
    }
}