using UnityEngine;
using System.Collections;

public class VisionCone : MonoBehaviour {

	public float speed = 1.0f;
	
	Vector3 directionToPlayer;
	Vector3 drawRayTop;
	Vector3 drawRayBot;

	Player player;

	float angleShift;

	public float minAngle = 225;
	public float maxAngle = 315;

	public float coneSize = 10;

	public float waitTime = 1.0f;

	private Blackboard bb;

	int direction = 1;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>();
		bb = GameObject.FindGameObjectWithTag ("Blackboard").GetComponent<Blackboard> ();

		directionToPlayer = (player.transform.position - transform.position).normalized;

		angleShift = minAngle;

		MoveCone ();
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

	private void MoveCone(){
		StartCoroutine(Move (direction));
	}

	private IEnumerator Wait(){
		yield return new WaitForSeconds (waitTime);

		StartCoroutine (Move (direction));
	}

	private IEnumerator Move(int pDirection){
//
	//		//this will make the enemy track the player
	//directionToPlayer = (player.transform.position - transform.position).normalized;

		while(pDirection == direction)
		{
			angleShift += direction * speed;
			//print ("angle : " + angleShift);
			
			if(angleShift >= 360)
				angleShift = 0;
			
			if((angleShift < minAngle && direction == -1) || (angleShift >= maxAngle && direction == 1))
				direction *= -1;
			
			directionToPlayer = Quaternion.Euler(0, 0, angleShift) * Vector3.right;
			
			drawRayTop = Quaternion.Euler (0, 0, angleShift + coneSize) * Vector3.right;
			drawRayBot = Quaternion.Euler (0, 0, angleShift - coneSize) * Vector3.right;
			
			//exact direction the enemy is looking
			//Debug.DrawRay (transform.position, directionToPlayer * 10, Color.green);


			yield return null;
		}

		yield return new WaitForSeconds(1f);
		
		StartCoroutine (Wait ());
	}

	void Update () {
		
		Debug.DrawRay (transform.position, drawRayTop * 10, Color.green);
		Debug.DrawRay (transform.position, drawRayBot * 10, Color.green);
		
		if(Mathf.Abs(CalcAngle(player.transform.position - transform.position)) < 20) {
			bb.GameOver();
		}
//		else
//			player.alive = true;
	}
}
//

	
	