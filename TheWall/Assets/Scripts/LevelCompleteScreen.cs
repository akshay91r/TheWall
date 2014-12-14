using UnityEngine;
using System.Collections;

public class LevelCompleteScreen : MonoBehaviour {
	
	public float timeToMoveIn = 0.5f;
	private Vector3 startPos;
	private GameObject timer;
	private Blackboard bb;
	private ProgressBar pb;
	
	private GameObject stateText;
	private GameObject scoreText;
	
	private int currentLevel;
	
	private bool restart = false;
	
	// Use this for initialization
	void Start () {
		
		GameObject bg = GameObject.FindGameObjectWithTag ("BG");
		bb = GameObject.FindGameObjectWithTag ("Blackboard").GetComponent<Blackboard> ();
		pb = GameObject.FindGameObjectWithTag ("ProgressBar").GetComponent<ProgressBar> ();
		stateText = GameObject.FindGameObjectWithTag ("StateText");
		scoreText = GameObject.FindGameObjectWithTag ("ScoreText");
		timer = GameObject.FindGameObjectWithTag ("Timer");
		startPos = new Vector3 (0, bg.GetComponent<SpriteRenderer>().bounds.size.y, 0);
		currentLevel = bb.GetCurrentLevel ();
		transform.position = startPos;
	}
	
	public void MoveIn()
	{
		timer.GetComponent<GUIText> ().enabled = false;
		GameObject medal = transform.Find ("Medal").gameObject;
		
		//int finalScore = bb.GetScore ();
		int achievement = pb.GetAchievement ();
		
		
		if(achievement == 0)
			medal.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(("ProgressBar/GreyTrophy"), typeof(Sprite));
		else if(achievement == 1)
			medal.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(("ProgressBar/BronzeTrophy"), typeof(Sprite));
		else if(achievement == 2)
			medal.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(("ProgressBar/SilverTrophy"), typeof(Sprite));
		else if(achievement == 3)
			medal.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(("ProgressBar/GoldTrophy"), typeof(Sprite));
		
		transform.Find ("Newspaper").gameObject.GetComponent<Newspaper> ().LoadRandomPage (currentLevel);
		
		//level passed
		if(achievement >= 1)
		{
			stateText.GetComponent<GUIText>().text = "Wall "+(currentLevel+1)+" Complete!";
			transform.Find ("NextLevelButton").gameObject.active = true;
		}
		//level failed
		else
		{
			stateText.GetComponent<GUIText>().text = "Try Again!";
			//reposition buttons
			transform.Find ("NextLevelButton").gameObject.active = false;
		}
		
		scoreText.GetComponent<GUIText> ().text = "Lives Saved: " + bb.GetScore ();
		
		StartCoroutine (Move (new Vector3 (0, 0, 0)));
	}
	
	public void MoveOut()
	{
		StartCoroutine (Move (startPos));
	}
	
	public void MoveOutAndRestart()
	{
		restart = true;
		StartCoroutine (Move (startPos));
	}
	
	private IEnumerator Move(Vector3 pos){
		
		//moving = true;
		
		float moveInSpeed = Vector3.Distance (transform.position, pos) / timeToMoveIn;
		
		while(Vector3.Distance(transform.position, pos) > 0.05f)
		{
			transform.position = Vector3.MoveTowards(transform.position, pos, moveInSpeed * Time.deltaTime);
			
			yield return null;
		}
		
		//moving = false;
		yield return new WaitForSeconds(0f);
		
		if(restart)
			Application.LoadLevel(Application.loadedLevel);
	}
}
