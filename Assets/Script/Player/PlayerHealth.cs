using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;  // HP สูงสุด
    private float currentHealth;    // HP ปัจจุบัน

    public float enemyDamage = 10f; // ดาเมจที่ได้รับเมื่อโดน Enemy
    public float damageCooldown = 1f; // คูลดาวน์ก่อนโดนดาเมจซ้ำ
    private float lastDamageTime;   // เวลาที่โดนดาเมจล่าสุด

    void Start()
    {
        currentHealth = maxHealth; // เริ่มเกมด้วย HP เต็ม
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) // ถ้า Player ชนศัตรู
        {
            TakeDamage(enemyDamage);
        }
    }

    void TakeDamage(float damage)
    {
        if (Time.time - lastDamageTime >= damageCooldown) // เช็คคูลดาวน์ก่อนลดเลือด
        {
            currentHealth -= damage;
            lastDamageTime = Time.time;

            Debug.Log("Player HP: " + currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        Debug.Log("Player Died!");
        // ใส่ระบบตาย เช่น รีเซ็ตเกม, แสดง UI, เล่นอนิเมชั่น ฯลฯ
    }
}