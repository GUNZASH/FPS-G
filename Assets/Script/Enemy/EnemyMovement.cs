using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private Transform player;  // อ้างอิงตำแหน่งของ Player
    public float detectionRange = 10f; // ระยะที่ศัตรูจะเริ่มไล่ล่า
    public float stopDistance = 1.5f; // ระยะที่ศัตรูจะหยุด (กันชน Player)

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // ดึง Component NavMeshAgent
        FindPlayer(); // หาตำแหน่ง Player อัตโนมัติ
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer(); // ค้นหา Player ใหม่ ถ้ายังไม่มีค่า
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            agent.SetDestination(player.position); // เดินไปที่ตำแหน่ง Player

            if (distance <= stopDistance)
            {
                agent.isStopped = true; // หยุดเดินเมื่อถึงระยะกำหนด
            }
            else
            {
                agent.isStopped = false; // เดินต่อไปถ้ายังไม่ถึง
            }
        }
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }
}