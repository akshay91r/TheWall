using UnityEngine;
using System.Collections;

public class RetryButton : MonoBehaviour {

	private bool active = false;
	private Blackboard bb;

	// Use this for initialization
	void Start () {

		bb = GameObject.FindGameObjectWithTag ("Blackboard").GetComponent<Blackboard> ();
	
	}

	public void Activate()
	{
		GetComponent<GUITexture> ().enabled = true;
		active = true;
	}

	void OnMouseDown()
	{
		if(active)
			bb.RestartLevel();
		//player.MoveToPosition (transform.position);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
