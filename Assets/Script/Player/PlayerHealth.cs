using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;  // HP สูงสุด
    private float currentHealth;    // HP ปัจจุบัน

    public float enemyDamage = 10f; // ดาเมจที่ได้รับเมื่อโดน Enemy
    public float damageCooldown = 1f; // คูลดาวน์ก่อนโดนดาเมจซ้ำ
    private float lastDamageTime;   // เวลาที่โดนดาเมจล่าสุด

    public TextMeshProUGUI healthText; // ตัวแปรเพื่อเชื่อมกับ TextMeshPro ใน UI

    void Start()
    {
        currentHealth = maxHealth; // เริ่มเกมด้วย HP เต็ม
        UpdateHealthUI(); // อัพเดต UI เมื่อเริ่มเกม
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) // ถ้า Player ชนศัตรู
        {
            TakeDamage(enemyDamage);
        }
    }

    public void TakeDamage(float damage)
    {
        if (Time.time - lastDamageTime >= damageCooldown) // เช็คคูลดาวน์ก่อนลดเลือด
        {
            currentHealth -= damage;
            lastDamageTime = Time.time;

            Debug.Log("Player HP: " + currentHealth);

            UpdateHealthUI(); // อัพเดต UI ทุกครั้งที่เลือดลด

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
        GetComponent<PlayerDeath>().HandleDeath();
    }

    // ฟังก์ชันสำหรับอัพเดตค่าเลือดใน UI
    void UpdateHealthUI()
    {
        healthText.text = "HP: " + currentHealth.ToString(); // แสดงค่าเลือดใน UI
    }
}