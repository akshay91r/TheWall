using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public float durationSeconds = 60;

	float startTime;
	float timeRemaining;
	string textTime;

	private bool gameOn = true;
	private int minutes;
	private int seconds;

	private Blackboard bb;

	// Use this for initialization
	void Start () {

		bb = GameObject.FindGameObjectWithTag ("Blackboard").GetComponent<Blackboard> ();
		timeRemaining = durationSeconds;
		startTime = Time.time;
	}

	// Update is called once per frame
	void Update () {

		if(gameOn)
		{

		var guiTime = Time.time - startTime;
		//The gui-Time is the difference between the actual time and the start time.
		minutes = (int)timeRemaining / 60; //Divide the guiTime by sixty to get the minutes.
		seconds = (int)timeRemaining % 60;//Use the euclidean division for the seconds.
		
		textTime = string.Format ("{0:00}:{1:00}", minutes, seconds); 
		//text.Time is the time that will be displayed.
		GetComponent<GUIText>().text = "Time left : " + textTime;

		if(timeRemaining <= 1)
			{
				gameOn = false;
				bb.GameOver ();
			}
		
		timeRemaining = durationSeconds - guiTime;
		}
	}
}

