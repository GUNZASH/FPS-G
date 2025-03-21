﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;  // ค่า HP สูงสุด
    private float currentHealth;    // ค่า HP ปัจจุบัน
    public Action OnDeath;


    void Start()
    {
        currentHealth = maxHealth; // เริ่มเกมด้วยค่า Max HP

        OnDeath += GetComponent<EnemyDeath>().HandleDeath; // เชื่อม OnDeath กับ EnemyDeath
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet")) // ตรวจจับกระสุน
        {
            TakeDamage(25f); // ลด HP 25 หน่วย (ปรับค่าตามต้องการ)
            Destroy(other.gameObject); // ลบกระสุนหลังโดน
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die(); // ถ้า HP <= 0 ให้ศัตรูตาย
        }
    }

    void Die()
    {
        Debug.Log("Enemy Died!");
        OnDeath?.Invoke();  // แจ้งว่า Enemy ตาย

        // เรียกใช้ EnemyDeath เพื่อเพิ่มคะแนน
        GetComponent<EnemyDeath>()?.HandleDeath();

        // ทำให้ศัตรูไม่เคลื่อนไหวเมื่อตาย
        GetComponent<EnemyMovement>().enabled = false;
        Destroy(gameObject);
    }

    public bool IsAlive()
    {
        return currentHealth > 0; // ถ้า HP ยังมากกว่า 0, หมายความว่ามีชีวิต
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth; // รีเซ็ต HP ให้เต็ม
    }
}