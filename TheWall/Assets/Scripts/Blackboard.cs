using UnityEngine;
using System.Collections;

public class Blackboard : MonoBehaviour {

	private Player player;
	private GameObject[] soldiers;

	private GUIText stateText;
	private RetryButton retry;

	// Use this for initialization
	void Start () {

		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		soldiers = GameObject.FindGameObjectsWithTag ("Soldier");
		stateText = GameObject.FindGameObjectWithTag ("StateText").GetComponent<GUIText> ();
		retry = GameObject.FindGameObjectWithTag ("RetryButton").GetComponent<RetryButton> ();
	}

	public void GameOver()
	{
		player.Die ();
		stateText.text = "You Lose";
		retry.Activate();
		foreach(GameObject g in soldiers)
			g.GetComponent<VisionCone>().StopAllCoroutines();

	}

	public void WinGame()
	{
		foreach(GameObject g in soldiers)
			g.GetComponent<VisionCone>().StopAllCoroutines();

		stateText.text = "You Win!";
		retry.Activate();
	}

	public void RestartLevel()
	{
		Application.LoadLevel (0);
	}
}
