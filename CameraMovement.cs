using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSCameraController : MonoBehaviour
{
    public float lookSpeed = 2.0f; // Speed of the camera look rotation
    public float moveSpeed = 5.0f; // Speed of movement
    public float jumpForce = 5.0f; // Force applied when jumping
    public float gravity = -9.81f; // Gravity strength

    private CharacterController characterController;
    private float verticalVelocity = 0f; // Vertical velocity for jumping
    private float verticalRotation = 0f; // Vertical rotation for the camera (up/down)
    public float upDownRange = 60.0f; // Clamping for up/down camera rotation

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        // Lock the cursor in the game window
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Confined;
        // Handle camera look (both horizontal and vertical rotations)
        HandleLook();

        // Handle player movement and jumping
        HandleMovement();
    }

    private void HandleLook()
    {
        // Horizontal rotation (left/right) applied to the whole object
        float horizontal = Input.GetAxis("Mouse X") * lookSpeed;
        transform.Rotate(0, horizontal, 0); // Rotate the entire object around the Y-axis

        // Vertical rotation (up/down) applied locally to the object
        verticalRotation -= Input.GetAxis("Mouse Y") * lookSpeed;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange); // Clamp vertical rotation

        // Apply the clamped vertical rotation to the object, affecting only pitch
        transform.localRotation = Quaternion.Euler(verticalRotation, transform.eulerAngles.y, 0f);
    }

    private void HandleMovement()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity = 0f; // Reset vertical velocity when on the ground

            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = jumpForce; // Apply jump force
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime; // Apply gravity over time
        }

        // Movement input (based on camera's forward and right direction)
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D or left/right arrow keys
        float moveVertical = Input.GetAxis("Vertical"); // W/S or up/down arrow keys

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Combine horizontal and vertical movement inputs
        Vector3 movement = (moveHorizontal * right + moveVertical * forward) * moveSpeed;
        movement.y = verticalVelocity; // Add vertical movement (jump/fall)

        // Move the character controller
        characterController.Move(movement * Time.deltaTime);
    }
}
