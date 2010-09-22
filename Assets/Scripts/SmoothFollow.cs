using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour
{

	//public
    public Transform target;
	public float distance = 10.0f;
    public float height = 5.0f;
    public float heightDamping = 1.0f;
    public float rotationDamping = 2.0f;

    //private
    private float wantedRotationAngle;
    private float wantedHeight;
    private float currentRotationAngle;
    private float currentHeight;

    void Update()
    {
        if (!target)  return;
        wantedRotationAngle = target.eulerAngles.y;
        wantedHeight = target.position.y + height;
        currentRotationAngle = transform.eulerAngles.y;
        currentHeight = transform.position.y;
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
        transform.position = target.position;
        transform.position -= currentRotation * Vector3.forward * distance;
        transform.position = SetY(transform.position, currentHeight);
        transform.LookAt(target);
    }

    private Vector3 SetY(Vector3 v3, float y)
    {
        v3.y = y;
        return v3;
    }
}
