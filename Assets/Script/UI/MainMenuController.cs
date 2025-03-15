using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // ฟังก์ชันเพื่อเปลี่ยนไป Scene1 (GamePlay)
    public void StartGame()
    {
        // โหลด Scene 1 (GamePlay)
        SceneManager.LoadScene("GamePlay");
    }

    // ฟังก์ชันเพื่อปิดเกม
    public void QuitGame()
    {
        // ปิดเกม
        Application.Quit();

        // สำหรับใน Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}