using UnityEngine;
using System.Collections;

public class VisionCone : MonoBehaviour {

	Vector3 directionToPlayer;
	Vector3 drawRayTop;
	Vector3 drawRayBot;

	Player player;

	float angleShift;

	private int angleCount;
	public string[] angleValues;

	public float speed = 1.0f;
	public float startAngle = 225;
	public float coneSize = 10;
	public float length = 5;
	public float waitTime = 1.0f;
	public float initialDelay = 0.5f; //initial time before starts sweeping

	private Blackboard bb;

	private int currentIndex = 0;
	private int direction = 1;
	private float newAngle;
	private int newDirection;

	private float currentAngle; 

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>();
		bb = GameObject.FindGameObjectWithTag ("Blackboard").GetComponent<Blackboard> ();

		directionToPlayer = (player.transform.position - transform.position).normalized;

		angleShift = startAngle;
		currentAngle = startAngle;

		DrawConeArea();

		StartCoroutine(StartConeMotion ());
	}



	private IEnumerator StartConeMotion(){

		print ("start motion called");

		drawRayTop = Quaternion.Euler (0, 0, angleShift + coneSize) * Vector3.right;
		drawRayBot = Quaternion.Euler (0, 0, angleShift - coneSize) * Vector3.right;

		yield return new WaitForSeconds (initialDelay);
		StartCoroutine(Move());
	}

	private IEnumerator Wait(){

		print ("wait called");

//		angleShift += direction * speed;
//		drawRayTop = Quaternion.Euler (0, 0, angleShift + coneSize) * Vector3.right;
//		drawRayBot = Quaternion.Euler (0, 0, angleShift - coneSize) * Vector3.right;
//
		yield return new WaitForSeconds (waitTime);

		StartCoroutine (Move());
	}

	private void GetNewAngleAndDirection()
	{
		//array not initialized
		if(angleValues.Length == 0)
		{
			print ("Array cannot be empty!");
			return;
		}

		print ("Current index checking : " + currentIndex);
		string[] splitString = angleValues[currentIndex].Split("-"[0]);
		
		newAngle = int.Parse (splitString[0]);
		newDirection = char.Parse (splitString[1]);

		print ("New angle : " + newAngle);
		print ("New direction : " + newDirection);
	}

	private IEnumerator Move(){
	
	//this will make the enemy track the player
	//directionToPlayer = (player.transform.position - transform.position).normalized;

		print ("move called");

		int pDirection;

		GetNewAngleAndDirection ();

		if(newDirection == 'a' || newDirection == 'A')
			pDirection = 1;
		else
			pDirection = -1;

		direction = pDirection;

//		if(startAngle == newAngle) //in case the starting angle is the same as the new target angle
//			direction *= -1;

		while(pDirection == direction)
		{
			if(angleShift == newAngle)
				direction *= -1;
			else
			{

				angleShift += direction * speed;
				print ("angle : " + angleShift);
			
				if(angleShift >= 360)
					angleShift = 0;

				else if(angleShift <= 0)
					angleShift = 360;
			}

			//direction defaults to 1

//			if(newAngle < currentAngle)
//			{
//				if((angleShift <= newAngle && direction == 1) || (angleShift >= currentAngle && direction == -1))
//					direction *= -1;
//			}
//			else if (newAngle > currentAngle)
//			{
//				if((angleShift >= newAngle && direction == -1) || (angleShift <= currentAngle && direction == 1))
//					direction *= -1;
//			}


			
			directionToPlayer = Quaternion.Euler(0, 0, angleShift) * Vector3.right;
			
			drawRayTop = Quaternion.Euler (0, 0, angleShift + coneSize) * Vector3.right;
			drawRayBot = Quaternion.Euler (0, 0, angleShift - coneSize) * Vector3.right;
			
			//exact direction the enemy is looking
			//Debug.DrawRay (transform.position, directionToPlayer * 10, Color.green);


			yield return null;
		}

		yield return new WaitForSeconds(1f);

		print ("exited loop");

		currentIndex++;

		if(currentIndex >= angleValues.Length)
			currentIndex = 0;
		
		StartCoroutine (Wait ());
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

	void Update () {
		
		Debug.DrawRay (transform.position, drawRayTop * length, Color.green);
		Debug.DrawRay (transform.position, drawRayBot * length, Color.green);

		//print ("End ray size : " + Vector3.Distance((drawRayTop * length), (drawRayBot * length)));

		//within sight distance and inside vision cone
		if((Vector3.Distance(transform.position, player.transform.position) <= length) && Mathf.Abs(CalcAngle(player.transform.position - transform.position)) < (coneSize*2)) {
			bb.GameOver();
		}
//		else
//			player.alive = true;
	}

	Mesh CreateMesh(float width, float height)
	{
		Mesh m = new Mesh();
		m.name = "ScriptedMesh";
		m.vertices = new Vector3[] {
			//			new Vector3(-width, -height/2, 0.01f),
			//			new Vector3(width, -height/2 - height, 0.01f),
			//			new Vector3(width, -height/2 + height, 0.01f),
			//			new Vector3(-width, -height/2, 0.01f)
			
			new Vector3(-width, 0, 0.01f),
			new Vector3(width, - height/2, 0.01f),
			new Vector3(width,  height/2, 0.01f),
			new Vector3(-width, 0, 0.01f)
		};
		m.uv = new Vector2[] {
			new Vector2 (0, 1),
			new Vector2 (0, 1),
			new Vector2(1, 1),
			new Vector2 (1, 0),
		};
		m.triangles = new int[] { 0, 1, 2 , 0, 1, 2};
		m.RecalculateNormals();
		
		return m;
	}
	
	private void DrawConeArea()
	{
		GameObject plane = new GameObject("Plane");
		MeshFilter meshFilter = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
		meshFilter.mesh = CreateMesh(length/2, 1.38f);
		//meshFilter.mesh = CreateMesh(length/2, length/2);
		MeshRenderer renderer = plane.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
		renderer.material.shader = Shader.Find ("Particles/Additive");
		Texture2D tex = new Texture2D(1, 1);
		tex.SetPixel(0, 0, Color.yellow);
		tex.Apply();
		renderer.material.mainTexture = tex;
		renderer.material.color = Color.yellow;
	}
}
//

	
	