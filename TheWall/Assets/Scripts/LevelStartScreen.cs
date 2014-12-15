using UnityEngine;
using System.Collections;

public class LevelStartScreen : MonoBehaviour {

	public float timeToMoveOut = 0.5f;

	private int currentLevel;
	private Vector3 outPos;
	private GameObject timer;
	private Blackboard bb;

	private string wall1Path = "Wall1/";
	private string wall2Path = "Wall2/";
	private string wall3Path = "Wall3/";
	private string wall4Path = "Wall4/";

	//keep track of intro screens
	private static bool[] levelIntroDone = new bool[4];

	//private ProgressBar pb;
	
	// Use this for initialization
	void Start () {

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
		if(!levelIntroDone[(currentLevel-1)]) //after adding a start screen, current level is (SceneNumber-1)
		{
			bb.LockAllNodes();
			LoadText();
			transform.position = new Vector3 (0, 0, 0);
		}
		else //start game immediately
			timer.GetComponent<Timer> ().StartTimer ();
	}

	private void LoadText()
	{
		GameObject leftText = transform.Find ("StartInfoLeft").gameObject;
		GameObject rightText = transform.Find ("StartInfoRight").gameObject;
		GameObject stateText = transform.Find ("StateText").gameObject;

		stateText.GetComponent<GUIText> ().text = "Wall " + currentLevel;

		if(currentLevel == 1)
		{
			leftText.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(string.Concat(wall1Path,"startInfoLeft"), typeof(Sprite));
			rightText.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(string.Concat(wall1Path,"startInfoRight"), typeof(Sprite));
		}
		else if(currentLevel == 2)
		{
			leftText.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(string.Concat(wall2Path,"startInfoLeft"), typeof(Sprite));
			rightText.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(string.Concat(wall2Path,"startInfoRight"), typeof(Sprite));
		}
		else if(currentLevel == 3)
		{
			leftText.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(string.Concat(wall3Path,"startInfoLeft"), typeof(Sprite));
			rightText.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(string.Concat(wall3Path,"startInfoRight"), typeof(Sprite));
		}
		else if(currentLevel == 4)
		{
			leftText.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(string.Concat(wall4Path,"startInfoLeft"), typeof(Sprite));
			rightText.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(string.Concat(wall4Path,"startInfoRight"), typeof(Sprite));
		}
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

		levelIntroDone [(currentLevel-1)] = true;
		bb.UnlockAllNodes ();

		//start timer when it moves out
		timer.GetComponent<Timer> ().StartTimer ();
	}
}
