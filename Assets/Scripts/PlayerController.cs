using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour
{

    public float speed = 5.0f;
    public float jumpSpeed = 5.00f;
    public float gravity = 9.6f;

    public float smoothSpeed = 10.0f;
    public float smoothDirection = 10.0f;

    public bool canJump = true;

    private Vector3 moveDirection = Vector3.zero;
    private float verticalSpeed = 0.0f;
    private float moveSpeed = 0.0f;

    private bool grounded = false;
    private bool jumping = false;

    private float targetAngle = 0.0f;


    void UpdateSmoothedMovementDirection()
    {
        Transform cameraTransform = Camera.main.transform;

        // Forward vector relative to the camera along the x-z plane	
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;
        // Right vector relative to the camera
        // Always orthogonal to the forward vector
        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        // Target direction relative to the camera
        Vector3 targetDirection = Input.GetAxis("Horizontal") * right + Input.GetAxis("Vertical") * forward;

        // We store speed and direction seperately,
        // so that when the character stands still we still have a valid forward direction
        // moveDirection is always normalized, and we only update it if there is user input.
        if (targetDirection != Vector3.zero)
        {
            moveDirection = Vector3.Lerp(moveDirection, targetDirection, smoothDirection * Time.deltaTime);
            moveDirection = moveDirection.normalized;
        }

        // Smooth the speed based on the current target direction
        float curSmooth = smoothSpeed * Time.deltaTime;
        // When in air we accelerate slower
        if (!grounded)
        {
            curSmooth *= 0.5f;
        }

        moveSpeed = Mathf.Lerp(moveSpeed, targetDirection.magnitude * speed, curSmooth);


    }

    void Update()
    {

        UpdateSmoothedMovementDirection();

        if (grounded)
        {
            verticalSpeed = 0.0f;

            // Jump		
            if (canJump && Input.GetButton("Jump"))
            {
                verticalSpeed = jumpSpeed;
                jumping = true;
                SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
            }
        }
        // Apply gravity
        verticalSpeed -= gravity * Time.deltaTime;

        Vector3 movement = moveDirection * moveSpeed + new Vector3(0, verticalSpeed, 0);
        movement *= Time.deltaTime;

        // Move the controller
        CharacterController controller = GetComponent<CharacterController>();
        CollisionFlags flags = controller.Move(movement);
        grounded = (flags & CollisionFlags.CollidedBelow) != 0;

        // Set rotation to the move direction	
        transform.rotation = Quaternion.LookRotation(moveDirection);

        // We are in jump mode but just became grounded
        if (grounded && jumping)
        {
            jumping = false;
            SendMessage("DidLand", SendMessageOptions.DontRequireReceiver);
        }
    }

    public float GetSpeed()
    {
        return moveSpeed;
    }

    public bool IsJumping()
    {
        return jumping;
    }

    public Vector3 GetDirection()
    {
        return moveDirection;
    }

}







