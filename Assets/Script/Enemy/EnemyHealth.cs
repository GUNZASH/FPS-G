using System.Collections;
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
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy Died!");
        OnDeath?.Invoke();
        GetComponent<EnemyDeath>().HandleDeath();
        Destroy(gameObject); // ลบศัตรูออกจากฉาก
    }
}