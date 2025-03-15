using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplode : MonoBehaviour
{
    private Animator anim;
    private bool isExploding = false;

    public float explosionDelay = 0.3f;
    public float explosionDamage = 20f;
    public float destroyDelay = 3f;

    public bool IsExploding => isExploding; // ใช้ตรวจสอบว่ากำลังระเบิดอยู่หรือไม่

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void TriggerExplosion(GameObject player)
    {
        if (isExploding) return; // ถ้ากำลังระเบิดอยู่แล้ว ให้ข้ามไป

        StartCoroutine(Explode(player));
    }

    IEnumerator Explode(GameObject player)
    {
        isExploding = true;
        Debug.Log("Explode triggered!");
        anim.SetTrigger("Explode");

        yield return new WaitForSeconds(explosionDelay);

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(explosionDamage);
        }

        anim.SetTrigger("Die");
        Debug.Log("Die triggered!");

        yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject);
    }
}