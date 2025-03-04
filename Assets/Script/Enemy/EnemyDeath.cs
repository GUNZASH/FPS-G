using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public int scoreValue = 10; // แต้มที่ Player จะได้รับเมื่อฆ่า Enemy
    private bool isDead = false; // เช็คว่า Enemy ตายแล้วหรือยัง

    public void HandleDeath()
    {
        if (isDead) return; // ป้องกันการให้แต้มซ้ำ
        isDead = true;

        Debug.Log("☠ Enemy Died! + " + scoreValue + " Points");
        ScoreManager.instance.AddScore(scoreValue); // เพิ่มแต้มให้ Player

        Destroy(gameObject); // ทำลาย Enemy
    }
}