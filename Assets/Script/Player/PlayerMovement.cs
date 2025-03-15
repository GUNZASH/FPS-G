using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float walkSpeed = 4f;
    public float runSpeed = 8f;
    public float crouchSpeed = 2f;
    public float jumpHeight = 1.2f;
    public float gravity = -9.81f;

    private float speed;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isCrouching = false;

    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    public Animator animator;  // ตัวแปรสำหรับควบคุม Animator

    void Start()
    {
        speed = walkSpeed;
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();  // ค้นหา Animator ใน child ของ Player
        }
    }

    void Update()
    {
        // ตรวจสอบว่าผู้เล่นอยู่บนพื้นหรือไม่
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // รีเซ็ตความเร็วแกน Y เมื่ออยู่บนพื้น
        }

        // รับค่าการกดปุ่มเดิน
        float x = Input.GetAxis("Horizontal"); // A, D
        float z = Input.GetAxis("Vertical");   // W, S

        // คำนวณทิศทางการเคลื่อนที่
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // Sprint (วิ่ง)
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            speed = runSpeed;
            animator.SetBool("isRunning", true);  // วิ่ง
        }
        else if (isCrouching)
        {
            speed = crouchSpeed;
            animator.SetBool("isRunning", false);  // ไม่วิ่ง
        }
        else
        {
            speed = walkSpeed;
            animator.SetBool("isRunning", false);  // ไม่วิ่ง
        }

        // เปลี่ยนแอนิเมชั่นการเดิน
        if (x == 0 && z == 0)
        {
            animator.SetBool("isIdle", true);  // ยืน
        }
        else
        {
            animator.SetBool("isIdle", false); // ไม่ยืน
        }

        // Jump (กระโดด)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Crouch (ย่อ)
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;
            controller.height = isCrouching ? 1.0f : 2.0f;
        }

        // แรงโน้มถ่วง
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
