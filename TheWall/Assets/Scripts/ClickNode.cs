using UnityEngine;
using System.Collections;

public class ClickNode : MonoBehaviour {

	private GameObject[] players;

	public int pathNo;
	public int nodeNo;

	public Sprite lockSprite;
	public Sprite unlockSprite;
	public bool unlocked = false;

	private Blackboard bb;

	// Use this for initialization
	void Start () {

		players = GameObject.FindGameObjectsWithTag ("Player");
		bb = GameObject.FindGameObjectWithTag ("Blackboard").GetComponent<Blackboard> ();
	
	}

	public void UnlockNode()
	{
		print ("UNLOCK CALLED" + nodeNo);
		if(!unlocked)
		{
			unlocked = true;

			GetComponent<SpriteRenderer>().sprite = unlockSprite;
		}
	}

	public void LockNode()
	{
		print ("LOCK CALLED" + nodeNo);
		if(unlocked)
		{
			unlocked = false;
			
			GetComponent<SpriteRenderer>().sprite = lockSprite;
		}
	}

	void OnMouseDown()
	{
		if(bb.isGameOver())
			return;

		if(unlocked)
		{
			foreach(GameObject g in players)
				g.GetComponent<Player>().MoveToPosition (pathNo, transform.position);
		}
	}

	public void HitByPlayer()
	{
		bb.HitNodeNumber(pathNo, nodeNo);
	}

	void OnTriggerEnter(Collider col) 
	{
		if(col.gameObject.tag == "Player")
			bb.HitNodeNumber(pathNo, nodeNo);
	}
}

//	void Update()
//	{
//		if(Input.GetMouseButtonDown(0))
//		{
//			print ("clickety");
//		}
//	}
