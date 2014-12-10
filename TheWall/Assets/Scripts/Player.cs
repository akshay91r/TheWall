using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public int path = 0; //default

	public float speed = 7.5f;
	private bool moving = false;


	public bool alive = true;
	private Sprite playerSprite;

	public Sprite human;
	public Sprite redHuman;

	private Blackboard bb;

	// Use this for initialization
	void Start () {

		playerSprite = GetComponent<SpriteRenderer> ().sprite;
		playerSprite = redHuman;
		bb = GameObject.FindGameObjectWithTag ("Blackboard").GetComponent<Blackboard> ();
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

	private IEnumerator Move(Vector3 pos){
		//
		
		while(Vector3.Distance(transform.position, pos) > 0.05f)
		{
			transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
			
			yield return null;
		}

		moving = false;
		yield return new WaitForSeconds(0f);
	}

	public void Die()
	{
		GetComponent<SpriteRenderer> ().sprite = redHuman;
		StopAllCoroutines ();
		bb.GameOver ();
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
