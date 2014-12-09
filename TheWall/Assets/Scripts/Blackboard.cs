using UnityEngine;
using System.Collections;

public class Blackboard : MonoBehaviour {

	private Player player;
	private GameObject[] soldiers;

	private GUIText stateText;
	private RetryButton retry;

	private int currentNode;
	private int currentPath;
	private int nodesInPath;
	private GameObject[] nodes;

	// Use this for initialization
	void Start () {

		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		soldiers = GameObject.FindGameObjectsWithTag ("Soldier");
		stateText = GameObject.FindGameObjectWithTag ("StateText").GetComponent<GUIText> ();
		retry = GameObject.FindGameObjectWithTag ("RetryButton").GetComponent<RetryButton> ();
		InitializeNodes ();
	}

	private void InitializeNodes()
	{
		nodes = GameObject.FindGameObjectsWithTag ("Node");
	}

	public void HitNodeNumber(int numHit)
	{
		print ("Node : " + numHit + " hit!");
		//print ("total : " + nodes.Length);

		if(numHit == (nodes.Length - 1))
			WinGame();
		else
		{
			for(int i = 0; i < nodes.Length; i++)
			{
				//unlock the next node
				if(nodes[i].GetComponent<ClickNode>().nodeNo == numHit + 1)
				{
					nodes[i].GetComponent<ClickNode>().UnlockNode();
				}

				//lock the last node
				if(nodes[i].GetComponent<ClickNode>().nodeNo == numHit)
				{
					nodes[i].GetComponent<ClickNode>().LockNode();
				}
			}
		}
	}

	public void GameOver()
	{
		player.Die ();
		stateText.text = "You Lose";
		retry.Activate();
		foreach(GameObject g in soldiers)
			g.GetComponent<CreateVisionCone>().StopAllCoroutines();

	}

	public void WinGame()
	{
		foreach(GameObject g in soldiers)
			g.GetComponent<CreateVisionCone>().StopAllCoroutines();

		stateText.text = "You Win!";
		retry.Activate();
	}

	public void RestartLevel()
	{
		Application.LoadLevel (0);
	}
}
