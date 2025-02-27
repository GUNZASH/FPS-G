using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public Camera playerCamera;

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // จำกัดมุมเงยหน้าก้ม
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // กล้องเงย-ก้ม
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // หมุนตัวละคร (ซ้าย-ขวา)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}