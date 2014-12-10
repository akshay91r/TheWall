using UnityEngine;
using System.Collections;

public class PatrolVision : MonoBehaviour {

	//Vector3 directionToPlayer;
	Vector3 drawRayTop;
	Vector3 drawRayBot;
	
	Player player;
	
	float angleFacing = 90;

	public float coneSize = 10;
	public float length = 5;

	public float angleGoingUp = 90;
	public float angleGoingDown = 270;
	//public float initialDelay = 0.5f; //initial time before starts sweeping
	
	private Blackboard bb;
	

	private int direction = 1;

	private Patrol patrol;
	
	private GameObject pivot;
	private GameObject visionCone;
	
	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>();
		bb = GameObject.FindGameObjectWithTag ("Blackboard").GetComponent<Blackboard> ();
		patrol = GetComponent<Patrol> ();
		pivot = transform.Find ("Pivot").gameObject;
		
		DrawConeArea();

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

	public void ChangeDirection()
	{
		if(angleFacing == angleGoingUp)
			angleFacing = angleGoingDown;
		else
			angleFacing = angleGoingUp;

		pivot.transform.rotation = Quaternion.Euler (0, 0, angleFacing);
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
		renderer.material = (Material)Resources.Load("YellowTex", typeof(Material));
		
		visionCone = plane;

		pivot.transform.position = transform.position;
		
		visionCone.transform.position = transform.position;
		visionCone.transform.position += new Vector3 ((visionCone.GetComponent<MeshRenderer> ().bounds.size.x)/2, 0, -0.5f);
		visionCone.AddComponent(typeof(MeshCollider));
		visionCone.GetComponent<MeshCollider>().isTrigger = true;
		visionCone.AddComponent(typeof(VisionCone));

		visionCone.transform.parent = pivot.transform;

		angleFacing = angleGoingUp;
		
		pivot.transform.rotation = Quaternion.Euler (0, 0, angleFacing);
	}	
}
