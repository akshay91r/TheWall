using UnityEngine;
using System.Collections;

public class Patrol : MonoBehaviour {

	public float speed = 5;
	public float waitTime = 1;
	public Vector3 targetPos = new Vector3 (-1.49f, -0.44f, 0);
	public float smallScalePercent = 0.75f;

	private Vector3 startScale;
	private Vector3 smallScale;
	private float rateOfScaleChange;

	private int direction = 1; //for up

	Vector3 startPos;

	// Use this for initialization
	void Start () {

		startScale = transform.localScale;
		smallScale = startScale * smallScalePercent;
		Calculations ();
		StartCoroutine(Move (targetPos, smallScale));
	}

	private void Calculations()
	{
		startPos = transform.position;
		float distanceCovered = (targetPos - startPos).magnitude;
		float timeTaken = distanceCovered/speed;

		float amountOfScaleChange = Mathf.Abs((smallScale - startScale).magnitude);
		rateOfScaleChange = amountOfScaleChange / timeTaken;
	}

	private IEnumerator Move(Vector3 pos, Vector3 targetScale){

		//position before moving
		startPos = transform.position;
		startScale = transform.localScale;
		
		while(Vector3.Distance(transform.position, pos) > 0.05f)
		{
			transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
			transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, rateOfScaleChange * Time.deltaTime);
			
			yield return null;
		}

		yield return new WaitForSeconds(0f);

		StartCoroutine(WaitAndMoveBack (startPos, startScale));
	}

	private IEnumerator WaitAndMoveBack(Vector3 pos, Vector3 scale)
	{
		GetComponent<PatrolVision> ().ChangeDirection ();
		yield return new WaitForSeconds (waitTime);
		direction *= -1;
		StartCoroutine(Move (pos, scale));
	}

	public int GetDirection(){
		return direction;
	}
}


// angles are in degrees, 000 is due north, Clockwise, left-handed.
// 315 around x is North and up 45.

