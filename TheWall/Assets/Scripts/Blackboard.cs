using UnityEngine;
using System.Collections;

public class Blackboard : MonoBehaviour {

	public int totalPaths = 1;

	private Player player;
	private GameObject[] soldiers;

	private GUIText stateText;
	private RetryButton retry;

	private int currentNode;
	private int currentPath;
	private int nodesInPath;
	private GameObject[,] nodes;

	private int[] pathLengths;
	private int pathsDone = 0;

	// Use this for initialization
	void Start () {

		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		soldiers = GameObject.FindGameObjectsWithTag ("Soldier");
		stateText = GameObject.FindGameObjectWithTag ("StateText").GetComponent<GUIText> ();
		retry = GameObject.FindGameObjectWithTag ("RetryButton").GetComponent<RetryButton> ();
		InitializeNodeArray ();
	}

	private void InitializeNodeArray()
	{
		//all the nodes
		GameObject[] linearArray = GameObject.FindGameObjectsWithTag ("Node");

		nodes = new GameObject[10,5];
		pathLengths = new int[10];

		print ("TOTAL LENGTH PATH 0 " + nodes.GetLength (0));
		print ("TOTAL LENGTH PATH 1 " + nodes.GetLength (1));
	

		for(int i = 0; i < linearArray.Length; i++)
		{
			ClickNode cn = linearArray[i].GetComponent<ClickNode>();
			pathLengths[cn.pathNo] ++;
		}

		for(int i = 0; i < linearArray.Length; i++)
		{
			ClickNode cn = linearArray[i].GetComponent<ClickNode>();
			//nodes[cn.pathNo,cn.nodeNo] = new GameObject();
			nodes[cn.pathNo,cn.nodeNo] = linearArray[i];
		}
	}

	public void HitNodeNumber(int pathNo, int numHit)
	{
		if(numHit == pathLengths[pathNo]-1)
			PathDone(pathNo);
			//WinGame();
		else
		{
			for(int i = 0; i < pathLengths[pathNo]; i++)
			{
				//unlock the next node
				print ("Path : " + pathNo);
				print ("Node no : " + i);

				if(nodes[pathNo,i].GetComponent<ClickNode>().nodeNo == numHit + 1)
				{
					nodes[pathNo,i].GetComponent<ClickNode>().UnlockNode();
				}

				//lock the last node
				if(nodes[pathNo,i].GetComponent<ClickNode>().nodeNo == numHit)
				{
					nodes[pathNo,i].GetComponent<ClickNode>().LockNode();
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

	private void PathDone(int pathNo)
	{
		stateText.text = "Path "+pathNo+" completed!";
		pathsDone++;
		print ("paths done : " + pathsDone);
		if(pathsDone == totalPaths)
			WinGame();
		else
			StartCoroutine(RemovePathCompleteText(1.5f));
	}

	private IEnumerator RemovePathCompleteText(float delay)
	{
		yield return new WaitForSeconds (delay);
		stateText.text = "";
	}

	private void WinGame()
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
