using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreSubmissionZone : MonoBehaviour
{
    public float timeLimitMinutes = 5f; // กำหนดเวลารอ (นาที) ปรับใน Inspector
    public TextMeshProUGUI timerText; // UI แสดงเวลาที่เหลือ
    public TextMeshProUGUI messageText; // ข้อความแจ้งเตือน
    public ParticleSystem effect; // Particle System ที่จะเปิดเมื่อเวลาครบ
    public GameObject leaderboardPanel; // Panel Leaderboard
    public TextMeshProUGUI leaderboardText; // แสดงอันดับแต้ม
    public Button continueButton; // ปุ่มเล่นต่อ
    public Button exitButton; // ปุ่มออกเกม

    private float remainingTime;
    private bool canSubmitScore = false;

    void Start()
    {
        remainingTime = timeLimitMinutes * 60f;
        StartCoroutine(CountdownTimer());
        leaderboardPanel.SetActive(false); // ซ่อน Panel ตอนเริ่มเกม
    }

    IEnumerator CountdownTimer()
    {
        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerUI();
            yield return null;
        }
        canSubmitScore = true;
        effect.Play(); // แสดง Particle System เมื่อถึงเวลา
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = $"Time Left: {minutes:D2}:{seconds:D2}";
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (canSubmitScore)
            {
                SubmitScore(ScoreManager.instance.score);
                ShowLeaderboard();
            }
            else
            {
                messageText.text = $"You need to wait {Mathf.CeilToInt(remainingTime / 60)} min {Mathf.CeilToInt(remainingTime % 60)} sec!";
            }
        }
    }

    void SubmitScore(int score)
    {
        List<int> highScores = LoadHighScores();
        highScores.Add(score);
        highScores.Sort((a, b) => b.CompareTo(a)); // เรียงลำดับจากมากไปน้อย
        if (highScores.Count > 10) highScores.RemoveAt(10); // จำกัดแค่ 10 อันดับ
        SaveHighScores(highScores);
    }

    List<int> LoadHighScores()
    {
        List<int> scores = new List<int>();
        for (int i = 0; i < 10; i++)
        {
            scores.Add(PlayerPrefs.GetInt($"HighScore{i}", 0));
        }
        return scores;
    }

    void SaveHighScores(List<int> scores)
    {
        for (int i = 0; i < scores.Count; i++)
        {
            PlayerPrefs.SetInt($"HighScore{i}", scores[i]);
        }
        PlayerPrefs.Save();
    }

    void ShowLeaderboard()
    {
        leaderboardPanel.SetActive(true);
        Time.timeScale = 0; // หยุดเกม

        // ปลดล็อคเมาส์เมื่อแสดงหน้าจอ Panel
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; // ทำให้เมาส์แสดง

        List<int> highScores = LoadHighScores();
        leaderboardText.text = "";
        for (int i = 0; i < highScores.Count; i++)
        {
            leaderboardText.text += $"{i + 1}. {highScores[i]}\n";
        }

        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(ResetGame);

        exitButton.onClick.RemoveAllListeners();
        exitButton.onClick.AddListener(QuitGame);
    }

    void ResetGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // รีเซ็ตทุกอย่าง

        // ล็อคเมาส์เมื่อเริ่มเกมใหม่
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // ซ่อนเมาส์
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
