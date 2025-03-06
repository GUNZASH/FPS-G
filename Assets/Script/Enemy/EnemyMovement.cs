using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private Transform player;  // อ้างอิงตำแหน่งของ Player
    public float detectionRange = 10f; // ระยะที่ศัตรูจะเริ่มไล่ล่า
    public float stopDistance = 1.5f; // ระยะที่ศัตรูจะหยุด (กันชน Player)
    public float moveSpeed = 3f; // ความเร็วในการเคลื่อนที่ของศัตรู

    void Start()
    {
        FindPlayer(); // ค้นหาผู้เล่นตอนเริ่มต้น
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer(); // ค้นหา Player ใหม่ ถ้ายังไม่มีค่า
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        // ถ้าผู้เล่นอยู่ในระยะที่สามารถตรวจจับได้
        if (distance <= detectionRange)
        {
            // เลื่อนศัตรูไปหาผู้เล่น
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            // ถ้าถึงระยะหยุดแล้ว (ระยะที่ศัตรูจะหยุดเคลื่อนที่)
            if (distance <= stopDistance)
            {
                // หยุดศัตรูที่จุดนี้
                // สามารถเพิ่มการทำงานอื่นๆ เช่น การโจมตี หรือทำอะไรที่ต้องการเมื่อถึงระยะนี้
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