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

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // เช็คว่ามีศัตรูไม่เกินจำนวนสูงสุด
            if (activeEnemies.Count < maxEnemies)
            {
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return; // ถ้าไม่มีจุดเกิด ให้หยุดทำงาน

        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)]; // เลือกจุดเกิดแบบสุ่ม
        GameObject newEnemy = Instantiate(enemyPrefab, randomSpawnPoint.position, Quaternion.identity);

        activeEnemies.Add(newEnemy); // บันทึกศัตรูที่เกิดขึ้น
        newEnemy.GetComponent<EnemyHealth>().OnDeath += () => activeEnemies.Remove(newEnemy); // ลบศัตรูออกจาก List เมื่อมันตาย
    }
}