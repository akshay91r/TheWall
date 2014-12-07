using UnityEngine;
using System.Collections;

public class VisionCone : MonoBehaviour {

	Vector3 directionToPlayer;
	Vector3 drawRayTop;
	Vector3 drawRayBot;

	Player player;

	float angleShift;

	public float speed = 1.0f;
	public float minAngle = 225;
	public float maxAngle = 315;
	public float coneSize = 10;
	public float length = 5;
	public float waitTime = 1.0f;
	public float initialDelay = 0.5f; //initial time before starts sweeping

	private Blackboard bb;

	int direction = 1;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>();
		bb = GameObject.FindGameObjectWithTag ("Blackboard").GetComponent<Blackboard> ();

		directionToPlayer = (player.transform.position - transform.position).normalized;

		angleShift = minAngle;

		DrawConeArea();

		StartCoroutine(StartConeMotion ());
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

	private IEnumerator StartConeMotion(){

		drawRayTop = Quaternion.Euler (0, 0, angleShift + coneSize) * Vector3.right;
		drawRayBot = Quaternion.Euler (0, 0, angleShift - coneSize) * Vector3.right;

		yield return new WaitForSeconds (initialDelay);
		StartCoroutine(Move (direction));
	}

	private IEnumerator Wait(){

//		angleShift += direction * speed;
//		drawRayTop = Quaternion.Euler (0, 0, angleShift + coneSize) * Vector3.right;
//		drawRayBot = Quaternion.Euler (0, 0, angleShift - coneSize) * Vector3.right;
//
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
		
		Debug.DrawRay (transform.position, drawRayTop * length, Color.green);
		Debug.DrawRay (transform.position, drawRayBot * length, Color.green);

		print ("End ray size : " + Vector3.Distance((drawRayTop * length), (drawRayBot * length)));

		//within sight distance and inside vision cone
		if((Vector3.Distance(transform.position, player.transform.position) <= length) && Mathf.Abs(CalcAngle(player.transform.position - transform.position)) < (coneSize*2)) {
			bb.GameOver();
		}
//		else
//			player.alive = true;
	}
}
//

	
	