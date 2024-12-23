using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonScrollController : MonoBehaviour
{
    Animator animator;

    public float moveSpeed = 5f;           // Speed of movement
    public float rotationSpeed = 100f;    // Speed of rotation for mouse scroll left/right
    public float lookSensitivity = 2f;    // Mouse look sensitivity for up/down
    public float jumpForce = 5f;          // Force applied when jumping
    public Transform cameraTransform;     // Reference to the camera transform
    public Transform headTransform;       // Reference to the head bone or object
    public float maxLookAngle = 63f;      // Max angle for looking up/down
    public float runModifier = 1.3f;
    public float followHeadSmoothness = 0.1f; // Determines the smoothness of the camera motion
    public float followHeadAmplitude = 0.5f;  // Determines how much the camera follows the head

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private float verticalRotation = 0f;
    private bool isGrounded = false;      // Tracks if the player is on the ground
    private Vector3 initialHeadToCameraOffset;
    private Quaternion initialHeadToCameraRotation;


    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        // Lock cursor to the game window
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Calculate the initial offset of the camera relative to the head
        initialHeadToCameraOffset = cameraTransform.position - headTransform.position;
        initialHeadToCameraRotation = Quaternion.Inverse(headTransform.rotation) * cameraTransform.rotation;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
        HandleJump();
        SmoothFollowHead();
    }

    private void HandleMovement()
    {
        // Get input for movement
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        float vertical = Input.GetAxisRaw("Vertical");     // W/S or Up/Down
        
        // Calculate direction relative to player orientation
        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
        Vector3 moveVelocity = moveDirection * moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveVelocity *= runModifier;
            animator.SetBool("isRunning", true);
        }
        else{
            animator.SetBool("isRunning", false);
        }
        // Apply velocity while preserving vertical velocity
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
        if(horizontal != 0.0f || vertical != 0.0f){
            animator.SetBool("isWalking", true);
        }
        else{
            animator.SetBool("isWalking", false);
        }
    }

    private void HandleMouseLook()
    {
        // Mouse input for rotation
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        // Rotate the player left/right
        transform.Rotate(Vector3.up * mouseX * rotationSpeed * Time.deltaTime);

        // Tilt the camera up/down
        verticalRotation -= mouseY * lookSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle); // Clamp tilt
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    private void HandleJump()
    {
        // Jump when spacebar is pressed and the player is grounded
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player is touching the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Reset grounded status when the player leaves the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void SmoothFollowHead()
{
    // Target position based on head position and initial offset
    Vector3 targetPosition = headTransform.TransformPoint(initialHeadToCameraOffset);

    // Target rotation based on head rotation and initial offset
    Quaternion targetRotation = headTransform.rotation * initialHeadToCameraRotation;

    // Smoothly move the camera
    cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, followHeadSmoothness);

    // Smoothly rotate the camera
    cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, targetRotation, followHeadSmoothness);
}
}
