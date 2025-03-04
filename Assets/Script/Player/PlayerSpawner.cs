using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform[] spawnPoints; // จุดเกิดของ Player (ใส่ใน Inspector)
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // ค้นหา Player ในฉาก
        if (player == null)
        {
            Debug.LogError("ไม่พบ Player ใน Hierarchy! โปรดกำหนด Tag เป็น 'Player'");
            return;
        }

        MovePlayerToSpawn(); // ย้าย Player ไปยังจุดเกิดเมื่อเริ่มเกม
    }

    public void MovePlayerToSpawn()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("ไม่มีจุดเกิดใน spawnPoints[]! โปรดกำหนดจุดเกิดใน Inspector");
            return;
        }

        int randomIndex = Random.Range(0, spawnPoints.Length); // สุ่มจุดเกิด
        Vector3 spawnPosition = spawnPoints[randomIndex].position;
        Quaternion spawnRotation = spawnPoints[randomIndex].rotation;

        player.transform.position = spawnPosition; // ย้าย Player ไปที่จุดเกิด
        player.transform.rotation = spawnRotation; // ปรับทิศทางของ Player
    }
}