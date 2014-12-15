using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public int path = 0; //default

	public bool swimming = false;
	public bool zipLine = false;
	
	public float speed = 7.5f;
	public float timeToMoveIn = 0.5f;
	private bool moving = false;
	
	public float corpseTime = 1;

	private float playerScale;
	
	public bool alive = true;
	private Sprite playerSprite;
	
	public Sprite human;
	public Sprite redHuman;
	
	private Blackboard bb;
	private SoundManager sm;
	
	// Use this for initialization
	void Start () {
		
		playerSprite = GetComponent<SpriteRenderer> ().sprite;
		playerSprite = redHuman;
		playerScale = transform.localScale.x;
		bb = GameObject.FindGameObjectWithTag ("Blackboard").GetComponent<Blackboard> ();
		sm = GameObject.FindGameObjectWithTag ("SoundManager").GetComponent<SoundManager> ();
	}
	
	public void MoveToPosition(int pPath, Vector3 pos)
	{
		//if already moving, or got called from a node on a different path
		if(moving || (pPath != path))
		{
			return;
		}
		
		moving = true;
		StartCoroutine(Move (pos));
	}

	public void ZiplineToPosition(int pPath, Vector3 pos)
	{
		//if already moving, or got called from a node on a different path
		if(moving || (pPath != path))
		{
			return;
		}
		
		moving = true;
		GetComponent<SplineController> ().FollowSpline ();
	}
	
	private IEnumerator Move(Vector3 pos){

		float multiplier = playerSprite.bounds.size.y / 2.6f;

		//float offset = playerSprite.bounds.size.y / 2.6f;

		float offset = multiplier * playerScale;

		//print ("offset = " + offset);

		pos += new Vector3 (0, offset, 0);

		while(Vector3.Distance(transform.position, pos) > 0.05f)
		{
			transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
			
			yield return null;
		}
		
		moving = false;
		yield return new WaitForSeconds(0f);
	}
	
	public void GetIn(Vector3 pos)
	{
		StartCoroutine (GetIntoStartPos (pos));
	}
	
	private IEnumerator GetIntoStartPos(Vector3 pos){
		
		moving = true;
		
		float moveInSpeed = Vector3.Distance (transform.position, pos) / timeToMoveIn;
		
		while(Vector3.Distance(transform.position, pos) > 0.05f)
		{
			transform.position = Vector3.MoveTowards(transform.position, pos, moveInSpeed * Time.deltaTime);
			
			yield return null;
		}
		
		moving = false;
		yield return new WaitForSeconds(0f);

		if(zipLine)
			GetComponent<SplineController>().CalculateDuration();
	}
	
	public void Die()
	{
		if(alive)
		{
			alive = false;
			GetComponent<SpriteRenderer> ().sprite = redHuman;
			StopAllCoroutines ();

			if(swimming)
				sm.PlayFoleySound(1);
			else
				sm.PlayDefectorDyingSound();

			bb.SpawnPlayerAtPath (path);
			StartCoroutine (RemoveAfterTime ());

			if(zipLine)
			{
				GetComponent<SplineInterpolator>().enabled = false;	
			}
		}
	}
	
	public void GetOut()
	{
		StartCoroutine (GetOutOfScreen ());
	}
	
	public IEnumerator GetOutOfScreen()
	{
		sm.PlayFoleySound (7);

		moving = true;
		Vector3 OutScreenPos = transform.position;
		OutScreenPos.x -= 10;
		GetComponent<Collider> ().enabled = false; //so that player doesnt get hit on way out
		
		float moveOutSpeed = Vector3.Distance (transform.position, OutScreenPos) / timeToMoveIn;
		
		while(Vector3.Distance(transform.position, OutScreenPos) > 0.15f)
		{
			transform.position = Vector3.MoveTowards(transform.position, OutScreenPos, moveOutSpeed * Time.deltaTime);
			
			yield return null;
		}
		
		moving = false;
		yield return new WaitForSeconds(0f);
		Destroy (gameObject);
	}
	
	private IEnumerator RemoveAfterTime()
	{
//		var desiredRotation = Quaternion.Euler (-90, 0, 0);
//		print ("Difference = " + Quaternion.Angle (transform.rotation, desiredRotation));
//		float turningSpeed = Quaternion.Angle (transform.rotation, desiredRotation) / 2;
//
//		while(Quaternion.Angle(transform.rotation, desiredRotation) != 0)
//		{
//			print ("Angle = " + Quaternion.Angle(transform.rotation, desiredRotation));
//			transform.rotation = Quaternion.Slerp (transform.rotation, desiredRotation, Time.time);
//			
//			yield return null;
//		}
//
//		print ("Courinte done");

		yield return new WaitForSeconds (corpseTime);
		Destroy (gameObject);
	}
	
	void OnCollisionEnter(Collision col) 
	{
		if(col.gameObject.tag == "Node")
			col.gameObject.GetComponent<ClickNode>().HitByPlayer();
	}

	void FixedUpdate()
	{
		//constantly move by zero to keep picking up collisions
		rigidbody.AddForce (0, 0, 0);
	}
}
