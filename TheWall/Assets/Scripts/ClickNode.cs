﻿using UnityEngine;
using System.Collections;

public class ClickNode : MonoBehaviour {

	private Player player;

	public int pathNo;
	public int nodeNo;

	public Sprite lockSprite;
	public Sprite unlockSprite;
	public bool unlocked = false;

	private Blackboard bb;

	// Use this for initialization
	void Start () {

		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
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
		if(unlocked)
			player.MoveToPosition (transform.position);
	}

	void OnTriggerEnter(Collider col) 
	{
		if(col.gameObject.tag == "Player")
			bb.HitNodeNumber(nodeNo);
	}
}

//	void Update()
//	{
//		if(Input.GetMouseButtonDown(0))
//		{
//			print ("clickety");
//		}
//	}
