using UnityEngine;
using System.Collections;

public class VisionCone : MonoBehaviour {

	public Sprite normalSoldier;
	public Sprite redSoldier;

	public float speed = 1.0f;
	
	Vector3 directionToPlayer;

	GameObject player;

	float angleShift;

	public float minAngle = 225;
	public float maxAngle = 315;

	public float coneSize = 10;

	int direction = 1;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player");

		directionToPlayer = (player.transform.position - transform.position).normalized;

		angleShift = minAngle;
	}

	private float CalcAngle(Vector3 newDirection) {
		// the vector that we want to measure an angle from
	
		Vector3 referenceForward = directionToPlayer;
		
		// the vector perpendicular to referenceForward (90 degrees clockwise)
		// (used to determine if angle is positive or negative)
		Vector3 referenceRight = transform.right;
		
		// Get the angle in degrees between 0 and 180
		float angle = Vector3.Angle(newDirection, referenceForward);
		
		// Determine if the degree value should be negative. Here, a positive value
		// from the dot product means that our vector is on the right of the reference vector
		// whereas a negative value means we're on the left.
		float sign = Mathf.Sign(Vector3.Dot(newDirection, referenceRight));
		
		return sign * angle;
	}
	
	// Update is called once per frame
	void Update () {

		Quaternion shiftAngle = Quaternion.Euler(0, angleShift, 0);

		//this will make the enemy track the player
		//directionToPlayer = (player.transform.position - transform.position).normalized;

		angleShift += direction * speed;
		print ("angle : " + angleShift);

		if(angleShift >= 360)
			angleShift = 0;

		if((angleShift < minAngle && direction == -1) || (angleShift >= maxAngle && direction == 1))
			direction *= -1;

		directionToPlayer = Quaternion.Euler(0, 0, angleShift) * Vector3.right;

		Vector3 drawRayTop = Quaternion.Euler (0, 0, angleShift + coneSize) * Vector3.right;
		Vector3 drawRayBot = Quaternion.Euler (0, 0, angleShift - coneSize) * Vector3.right;

		//exact direction the enemy is looking
		//Debug.DrawRay (transform.position, directionToPlayer * 10, Color.green);
		Debug.DrawRay (transform.position, drawRayTop * 10, Color.green);
		Debug.DrawRay (transform.position, drawRayBot * 10, Color.green);

		if(Mathf.Abs(CalcAngle(player.transform.position - transform.position)) < 20) {
			GetComponent<SpriteRenderer>().sprite = redSoldier;
		}
		else
			GetComponent<SpriteRenderer>().sprite = normalSoldier;

	}
}
