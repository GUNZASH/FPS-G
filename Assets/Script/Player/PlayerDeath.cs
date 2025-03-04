using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour
{
    public GameObject deathUI; // UI ที่จะแสดงตอน Player ตาย
    public Transform respawnPoint; // จุดเกิดใหม่ของ Player
    private CharacterController characterController;

    void Start()
    {
        if (deathUI != null)
        {
            deathUI.SetActive(false); // ซ่อน UI ตอนเริ่มเกม
        }

        characterController = GetComponent<CharacterController>();

        if (characterController == null)
        {
            Debug.LogError("❌ ไม่พบ CharacterController ใน Player!");
        }

        LockCursor(true); // ล็อกเมาส์เมื่อเริ่มเกม
    }

    public void HandleDeath()
    {
        Debug.Log("💀 Player Died!");
        Time.timeScale = 0; // หยุดเกม
        deathUI.SetActive(true); // แสดง UI ตาย
        LockCursor(false); // ปลดล็อกเมาส์ให้ใช้งาน UI ได้
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // ทำให้เกมเล่นต่อได้
        ResetGameState(); // รีเซ็ตค่าทุกอย่าง

        if (respawnPoint != null)
        {
            // ย้าย Player ไปจุด Respawn
            characterController.enabled = false; // ปิด CharacterController ก่อนย้ายตำแหน่ง
            transform.position = respawnPoint.position;
            transform.rotation = respawnPoint.rotation;
            characterController.enabled = true; // เปิด CharacterController อีกครั้ง
        }
        else
        {
            Debug.LogError("❌ ไม่มี Respawn Point!");
        }

        deathUI.SetActive(false); // ซ่อน UI ตาย
        LockCursor(true); // ล็อกเมาส์กลับหลังเกิดใหม่
    }

    public void QuitGame()
    {
        Debug.Log("🚪 ออกจากเกม!");
        Application.Quit();
    }

    private void LockCursor(bool isLocked)
    {
        Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isLocked;
    }
    private void ResetGameState()
    {
        // รีเซ็ตคะแนน
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.score = 0;
            ScoreManager.instance.AddScore(0); // อัปเดต UI
        }

        // รีเซ็ตกระสุนของ AK47
        AK47 ak47 = FindObjectOfType<AK47>();
        if (ak47 != null)
        {
            ak47.ResetAmmo();
        }
    }
}