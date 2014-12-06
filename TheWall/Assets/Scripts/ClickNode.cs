using UnityEngine;
using System.Collections;

public class ClickNode : MonoBehaviour {

	private Player player;

	// Use this for initialization
	void Start () {

		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
	
	}

	void OnMouseDown()
	{
		//print ("clickety");
		player.MoveToPosition (transform.position);
	}
}

//	void Update()
//	{
//		if(Input.GetMouseButtonDown(0))
//		{
//			print ("clickety");
//		}
//	}
