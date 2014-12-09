using UnityEngine;
using System.Collections;

public class CreateVisionCone : MonoBehaviour {

	Vector3 directionToPlayer;
	Vector3 drawRayTop;
	Vector3 drawRayBot;

	Player player;

	float angleShift;

	private int angleCount;
	public string[] angleValues;

	//public string coneTexPath;

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

	private GameObject pivot;
	private GameObject visionCone;
	private Vector3 rotatePoint;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>();
		bb = GameObject.FindGameObjectWithTag ("Blackboard").GetComponent<Blackboard> ();
		pivot = transform.Find ("Pivot").gameObject;

		directionToPlayer = (player.transform.position - transform.position).normalized;

		angleShift = startAngle;
		currentAngle = startAngle;

		DrawConeArea();

		StartCoroutine(StartConeMotion ());
	}



	private IEnumerator StartConeMotion(){

		drawRayTop = Quaternion.Euler (0, 0, angleShift + coneSize) * Vector3.right;
		drawRayBot = Quaternion.Euler (0, 0, angleShift - coneSize) * Vector3.right;

		yield return new WaitForSeconds (initialDelay);

		StartCoroutine(Move());
	}

	private IEnumerator Wait(){

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

		//print ("Current index checking : " + currentIndex);
		string[] splitString = angleValues[currentIndex].Split("-"[0]);
		
		newAngle = float.Parse (splitString[0]);
		newDirection = char.Parse (splitString[1]);
	}

	private IEnumerator Move(){
	
	//this will make the enemy track the player
	//directionToPlayer = (player.transform.position - transform.position).normalized;

		int pDirection;

		GetNewAngleAndDirection ();

		if(newDirection == 'a' || newDirection == 'A')
			pDirection = 1;
		else
			pDirection = -1;

		direction = pDirection;

		print ("Angle Shift : " + angleShift);
		print ("New angle : " + newAngle);
	
		while(pDirection == direction)
		{
			//if(angleShift == newAngle)
			if(Mathf.Abs(angleShift - newAngle) < 1.0f)
				direction *= -1;
			else
			{

				angleShift += direction * speed;
				//print ("angle : " + angleShift);
			
				if(angleShift >= 360)
					angleShift = 0;

				else if(angleShift <= 0)
					angleShift = 360;
			}
			
			directionToPlayer = Quaternion.Euler(0, 0, angleShift) * Vector3.right;

			drawRayTop = Quaternion.Euler (0, 0, angleShift + coneSize) * Vector3.right;
			drawRayBot = Quaternion.Euler (0, 0, angleShift - coneSize) * Vector3.right;


			//this works
			pivot.transform.rotation = Quaternion.Euler(0,0,angleShift);

			//pivot.transform.RotateAround(rotatePoint, Vector3.forward * direction, 30 * Time.deltaTime);


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
//		if((Vector3.Distance(transform.position, player.transform.position) <= length) && Mathf.Abs(CalcAngle(player.transform.position - transform.position)) < (coneSize*2)) {
//			bb.GameOver();
//		}
//
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
			new Vector3(width, - height, 0.01f),
			new Vector3(width,  height, 0.01f),
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
		GameObject plane = new GameObject("VisionCone");
		MeshFilter meshFilter = (MeshFilter)plane.AddComponent(typeof(MeshFilter));

		float newLength = length / 2;

		//height first, then width (dont ask)
		meshFilter.mesh = CreateMesh(newLength, coneSize/4);
		MeshRenderer renderer = plane.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
		renderer.material.shader = Shader.Find ("Particles/Additive");

		renderer.sortingLayerName = "Player";
		renderer.sortingOrder = 2;


		Vector3[]  vertices = meshFilter.mesh.vertices;
		Vector2[] uvs = new Vector2[vertices.Length];

		for (int i = 0 ; i < uvs.Length; i++)
			uvs[i] = new Vector2 (vertices[i].x, vertices[i].y);

		meshFilter.mesh.uv = uvs;
		renderer.material = (Material)Resources.Load("GreenTex", typeof(Material));

		visionCone = plane;
		//visionCone.transform.parent = transform;

		pivot.transform.position = transform.position;

		visionCone.transform.position = transform.position;
		visionCone.transform.position += new Vector3 ((visionCone.GetComponent<MeshRenderer> ().bounds.size.x)/2, 0, -0.5f);
		visionCone.AddComponent(typeof(MeshCollider));
		visionCone.AddComponent(typeof(VisionCone));


		visionCone.transform.parent = pivot.transform;

		pivot.transform.rotation = Quaternion.Euler (0, 0, angleShift);
	
		//visionCone.transform.RotateAround(rotatePoint, Vector3.forward * direction, 30 * Time.deltaTime);

		rotatePoint = transform.position;
	}	
}
//

	
	