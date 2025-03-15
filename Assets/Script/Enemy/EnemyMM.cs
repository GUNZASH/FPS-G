using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMM : MonoBehaviour
{
    private Transform player;
    private Animator anim;

    public float detectionRange = 10f;
    public float stopDistance = 1.5f;
    public float moveSpeed = 3f;
    public float detectionRadius = 2f;

    void Start()
    {
        anim = GetComponent<Animator>();
        FindPlayer();
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }

        MoveTowardsPlayer();
        DetectPlayer();
    }

    void MoveTowardsPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        LookAtPlayer();

        if (distance <= detectionRange && distance > stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            anim.SetBool("isWalking", true); // ✅ เก็บไว้เฉพาะท่าเดิน
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    void DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                GetComponent<EnemyExplode>()?.TriggerExplosion(hit.gameObject);
                break;
            }
        }
    }

    void LookAtPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-directionToPlayer, Vector3.up);
            transform.rotation = targetRotation;
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}