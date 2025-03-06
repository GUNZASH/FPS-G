using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab ศัตรู
    public Transform[] spawnPoints; // จุดเกิดศัตรู (ลากจาก Inspector)

    public float spawnInterval = 5f; // เวลาที่จะ Spawn ศัตรูใหม่
    public int maxEnemies = 10; // จำนวนศัตรูสูงสุดในฉาก

    private List<GameObject> activeEnemies = new List<GameObject>(); // เก็บศัตรูที่เกิดแล้ว
    private int currentSpawnIndex = 0; // ใช้ติดตามการเกิดศัตรูในแต่ละจุด

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // เช็คว่ามีศัตรูไม่เกินจำนวนสูงสุด และเช็คว่าศัตรูที่ตายแล้วถูกลบออกจาก activeEnemies หรือไม่
            activeEnemies.RemoveAll(enemy => enemy == null); // ลบศัตรูที่ตายแล้วออกจากรายการ

            if (activeEnemies.Count < maxEnemies)
            {
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return; // ถ้าไม่มีจุดเกิด ให้หยุดทำงาน

        // ถ้าทุกจุดเกิดถูกใช้หมดแล้ว ให้กลับมาเริ่มใหม่จากจุดแรก
        if (currentSpawnIndex >= spawnPoints.Length)
        {
            currentSpawnIndex = 0;
        }

        // เลือกจุดเกิดถัดไปที่ยังไม่เคยใช้
        Transform spawnPoint = spawnPoints[currentSpawnIndex];

        // สร้างศัตรูที่จุดเกิดนั้น
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        activeEnemies.Add(newEnemy); // บันทึกศัตรูที่เกิดขึ้น
        newEnemy.GetComponent<EnemyHealth>().OnDeath += () =>
        {
            activeEnemies.Remove(newEnemy); // ลบศัตรูออกจาก List เมื่อมันตาย
            Destroy(newEnemy); // ลบศัตรูออกจากฉาก
        };

        // เพิ่มตัวแปร index เพื่อให้ไปเกิดที่จุดถัดไป
        currentSpawnIndex++;
    }
}