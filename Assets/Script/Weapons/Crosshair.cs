using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public Color crosshairColor = Color.white;
    public float crosshairSize = 10f;
    public float thickness = 2f;

    void OnGUI()
    {
        float centerX = Screen.width / 2;
        float centerY = Screen.height / 2;

        GUI.color = crosshairColor;

        // Vertical Line
        GUI.DrawTexture(new Rect(centerX - thickness / 2, centerY - crosshairSize, thickness, crosshairSize * 2), Texture2D.whiteTexture);

        // Horizontal Line
        GUI.DrawTexture(new Rect(centerX - crosshairSize, centerY - thickness / 2, crosshairSize * 2, thickness), Texture2D.whiteTexture);
    }
}
