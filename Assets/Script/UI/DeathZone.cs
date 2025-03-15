using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public float deathDamage = 999f;  // ดาเมจที่ทำให้ Player ตายทันที

    void OnTriggerEnter(Collider other)
    {
        // เช็คว่าเป็น Player หรือไม่
        if (other.CompareTag("Player"))
        {
            // ส่งดาเมจให้กับ Player
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(deathDamage);  // ให้ดาเมจ 999
            }
        }
    }
}