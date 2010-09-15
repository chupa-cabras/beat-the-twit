









var speed = 1.0;
var jumpSpeed = 0.00;
var gravity = 9.0;

var smoothSpeed = 10.0;
var smoothDirection = 10.0;

var canJump = true;

private var moveDirection = Vector3.zero;
private var verticalSpeed = 0.0;
private var moveSpeed = 0.0;

private var grounded : boolean = false;
private var jumping : boolean = false;

private var targetAngle = 0.0;

// Require a character controller to be attached to the same game object
@script RequireComponent(CharacterController)

function Awake ()
{
	moveDirection = transform.TransformDirection(Vector3.forward);
}

function UpdateSmoothedMovementDirection ()
{
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

function Update() {

	UpdateSmoothedMovementDirection();

	if (grounded) {
		verticalSpeed = 0.0;
		
		// Jump		
		if (canJump && Input.GetButton ("Jump")) {
			verticalSpeed = jumpSpeed;
			jumping = true;
			SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
		}
	}
	// Apply gravity
	verticalSpeed -= gravity * Time.deltaTime;
	
	var movement = moveDirection * moveSpeed + Vector3 (0, verticalSpeed, 0);
	movement *= Time.deltaTime;
	
	// Move the controller
	var controller : CharacterController = GetComponent(CharacterController);
	var flags = controller.Move(movement);
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

function GetSpeed () {
	return moveSpeed;
}

function IsJumping () {
	return jumping;
}

function GetDirection () {
	return moveDirection;
}

