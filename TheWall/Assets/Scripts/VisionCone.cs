using UnityEngine;
using System.Collections;

public class VisionCone : MonoBehaviour {

	private Blackboard bb;

	// Use this for initialization
	void Start () {
	
		bb = GameObject.FindGameObjectWithTag ("Blackboard").GetComponent<Blackboard> ();

	}

	void OnCollisionEnter(Collision col) 
	{
		if(col.gameObject.tag == "Player")
			bb.GameOver();
	}
}
