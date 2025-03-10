using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject grenadePrefab; // พรีแฟบของระเบิด
    public Transform throwPoint; // จุดที่ปาระเบิด (เช่น มือของผู้เล่น)
    public float throwForce = 10f; // แรงปาระเบิด
    public float maxHoldTime = 2f; // เวลาสูงสุดที่กดปาเพื่อเพิ่มแรง
    private float holdTime = 0f;

    void Update()
    {
        // กดคลิกซ้ายค้างเพื่อชาร์จแรง
        if (Input.GetMouseButtonDown(0))
        {
            holdTime = 0f;
        }

        if (Input.GetMouseButton(0))
        {
            holdTime += Time.deltaTime;
            holdTime = Mathf.Clamp(holdTime, 0f, maxHoldTime);
        }

        // ปาระเบิดเมื่อปล่อยคลิกซ้าย
        if (Input.GetMouseButtonUp(0))
        {
            ThrowGrenade();
            Debug.Log("โยนระเบิด!"); // ✅ เช็คว่าฟังก์ชันทำงานหรือไม่
        }
    }

    void ThrowGrenade()
    {
        // สร้างระเบิดออกไปจากตำแหน่ง throwPoint
        GameObject grenade = Instantiate(grenadePrefab, throwPoint.position, throwPoint.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();

        // ตั้งค่า isKinematic เป็น true ก่อนเพื่อไม่ให้ระเบิดตกไปเอง
        rb.isKinematic = true;

        // ปรับค่า mass และ drag เพื่อให้ระเบิดมีน้ำหนักที่เหมาะสม
        rb.mass = 1f; // น้ำหนักระเบิด
        rb.drag = 0.2f; // การต้านทานอากาศ

        // คำนวณทิศทางการโยน (ทำให้ระเบิดมีมุมขึ้นเล็กน้อย)
        Vector3 throwDirection = (throwPoint.forward + throwPoint.up * 0.5f).normalized;

        // ปรับแรงโยน (finalForce) โดยคำนวณจาก holdTime
        float finalForce = throwForce + (holdTime / maxHoldTime) * throwForce * 3f; // เพิ่มความแรงจากเวลา hold

        // ทำให้ระเบิดหายไปจากมือ
        this.gameObject.SetActive(false); // ลบระเบิดในมือหลังจากโยนออกไป

        // เปิดใช้งานระเบิดทันทีหลังจากโยน
        grenade.SetActive(true); // เปิดการใช้งานระเบิดที่ถูกสร้างขึ้น

        // ให้ระเบิดไม่ตกลงทันที: เปิดใช้แรงโน้มถ่วงและเพิ่มแรง
        rb.isKinematic = false;  // เปลี่ยนเป็น false หลังจากโยนออกไป
        rb.AddForce(throwDirection * finalForce, ForceMode.Impulse);  // เพิ่มแรงให้กับระเบิด

        // เปิดใช้งานการใช้แรงโน้มถ่วง
        rb.useGravity = true;

        // เรียกใช้งานการระเบิดทันที
        grenade.GetComponent<GrenadeExplosion>().Activate(holdTime); // ส่งค่า holdTime ไปที่การระเบิด
    }
}