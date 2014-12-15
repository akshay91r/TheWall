using UnityEngine;
using System.Collections;

public class WallFallScreen : MonoBehaviour {

	public float timeToMoveOut = 0.5f;
	private Vector3 outPos;


	// Use this for initialization
	void Start () {
		
		GameObject bg = GameObject.FindGameObjectWithTag ("BG");
		outPos = new Vector3 (bg.GetComponent<SpriteRenderer>().bounds.size.x, 0, 0);
	}
	
	public void MoveOut()
	{
		StartCoroutine (Move (outPos));
	}
	
	private IEnumerator Move(Vector3 pos){
		
		//moving = true;
		
		float moveInSpeed = Vector3.Distance (transform.position, pos) / timeToMoveOut;
		
		while(Vector3.Distance(transform.position, pos) > 0.05f)
		{
			transform.position = Vector3.MoveTowards(transform.position, pos, moveInSpeed * Time.deltaTime);
			
			yield return null;
		}
		
		//moving = false;
		yield return new WaitForSeconds(0f);
	}
}
