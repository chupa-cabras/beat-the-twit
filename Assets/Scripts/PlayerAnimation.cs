using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour
{
	
	
	public float walkSpeed;
	private CharacterController characterController;
	private float speed;
	private Vector3 moveDirection;
	
	public PlayerAnimation()
	{
		walkSpeed = 3f;
		characterController = GetComponent<CharacterController>(); 
		speed = 0f;
		moveDirection = Vector3.zero;
	}
	

	void Start()
	{
		animation.wrapMode = WrapMode.Loop;
		animation.Stop();
	}
	
	void Update()
	{
		if (Input.GetAxis("Vertical") > 0.2)
			animation.CrossFade("Run01");
		else
			animation.CrossFade("Idle1");
	}
}
