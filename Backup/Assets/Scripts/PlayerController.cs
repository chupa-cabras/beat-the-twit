using UnityEngine;
using System.Collections;












public class PlayerController : MonoBehaviour {

	public float speed = 1.0;
	public float jumpSpeed = 0.00;
	public float gravity = 9.6;
	
	public float smoothSpeed = 10.0;
	public float smoothDirection = 10.0;
	
	public bool canJump = true;
	
	private Vector3 moveDirection = Vector3.zero;
	private float verticalSpeed = 0.0;
	private float moveSpeed = 0.0;
	
	private bool grounded  = false;
	private bool jumping  = false;
	
	private float targetAngle = 0.0;
	
	// Use this for initialization
	void Awake () {
	
	}
	
	void UpdateSmoothedMovementDirection () {
		
		
		
	var cameraTransform = Camera.main.transform;
	
	// Forward vector relative to the camera along the x-z plane	
	var forward = cameraTransform.TransformDirection(Vector3.forward);
	forward.y = 0;
	forward = forward.normalized;
	// Right vector relative to the camera
	// Always orthogonal to the forward vector
	var right = Vector3(forward.z, 0, -forward.x);

	// Target direction relative to the camera
	var targetDirection = Input.GetAxis("Horizontal") * right + Input.GetAxis("Vertical") * forward;
	
	// We store speed and direction seperately,
	// so that when the character stands still we still have a valid forward direction
	// moveDirection is always normalized, and we only update it if there is user input.
	if (targetDirection != Vector3.zero)
	{
		moveDirection = Vector3.Lerp(moveDirection, targetDirection, smoothDirection * Time.deltaTime);
		moveDirection = moveDirection.normalized;
	}

	// Smooth the speed based on the current target direction
	var curSmooth = smoothSpeed * Time.deltaTime;
	// When in air we accelerate slower
	if (!grounded)
	{
		curSmooth *= 0.5;
	}

	moveSpeed = Mathf.Lerp(moveSpeed, targetDirection.magnitude * speed, curSmooth);

	
	}
}
