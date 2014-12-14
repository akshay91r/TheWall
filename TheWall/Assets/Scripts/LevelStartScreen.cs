using UnityEngine;
using System.Collections;

public class LevelStartScreen : MonoBehaviour {

	public float timeToMoveOut = 0.5f;

	private int currentLevel;
	private Vector3 outPos;
	private GameObject timer;
	private Blackboard bb;

	//keep track of intro screens
	private static bool[] levelIntroDone = new bool[4];

	//private ProgressBar pb;
	
	// Use this for initialization
	void Start () {

//		for(int i = 0; i < 4; i++)
//		{//first time - set all to false
//			levelIntroDone[i] = false;
//		}

		GameObject bg = GameObject.FindGameObjectWithTag ("BG");
		bb = GameObject.FindGameObjectWithTag ("Blackboard").GetComponent<Blackboard> ();
		timer = GameObject.FindGameObjectWithTag ("Timer");
		outPos = new Vector3 (bg.GetComponent<SpriteRenderer>().bounds.size.x, 0, 0);

		currentLevel = bb.GetCurrentLevel ();

//		print ("PRINTING ALL LEVEL DONE VALUES ");
//		for(int i = 0; i < 4; i++)
//		{//first time - set all to false
//			print ("LEVEL " + i + " DONE : " + levelIntroDone[i]);
//			//levelIntroDone[i] = false;
//		}

		//first time scene running
		if(!levelIntroDone[currentLevel])
			transform.position = new Vector3 (0, 0, 0);
		else //start game immediately
			timer.GetComponent<Timer> ().StartTimer ();
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

		levelIntroDone [currentLevel] = true;

		//start timer when it moves out
		timer.GetComponent<Timer> ().StartTimer ();
	}
}
