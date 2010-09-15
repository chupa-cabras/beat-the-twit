var landBounceTime = 0.6;
private var lastJump : AnimationState;
function Start ()
{
	// We are in full control here - don't let any other animations play when we start
//	animation.Stop();

	// By default loop all animations
	animation.wrapMode = WrapMode.Loop;

	// The jump animation is clamped and overrides all others
	var jump = animation["GayJump"];
	jump.layer = 1;
	jump.enabled = false;
	jump.wrapMode = WrapMode.Clamp;
}

function Update ()
{
	var marioController : SuperMarioController = GetComponent(SuperMarioController);
	var currentSpeed = marioController.GetSpeed();

	// Switch between idle and walk
	if (currentSpeed > 0.1)
		animation.CrossFade("Dance");
	else
		animation.CrossFade("Kick");

	// When we jump we want the character start animate the landing bounce, exactly when he lands. So we do this:
	// - pause animation (setting speed to 0) when we are jumping and the animation time is at the landBounceTime
	// - When we land we set the speed back to 1
	if (marioController.IsJumping())
	{
		if (lastJump.time > landBounceTime)
			lastJump.speed = 0;
	}
}

function DidJump () {
	// We want to play the jump animation queued,
	// so that we can play the jump animation multiple times, overlaying each other
	// We dont want to rewind the same animation to avoid sudden jerks!
	lastJump = animation.CrossFadeQueued("GayJump", 0.3, QueueMode.PlayNow);
}

function DidLand () {
	lastJump.speed = 1;
}