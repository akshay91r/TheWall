﻿using UnityEngine;
using System.Collections;

public class Blackboard : MonoBehaviour {
	
	public int totalPaths = 1;
	
	public GameObject playerObjPath0;
	public GameObject playerObjPath1;
	public GameObject playerObjPath2;
	private Vector3[] playerStartPositions;
	private Vector3[] playerStartScales;

	private ProgressBar pb;
	
	private Player player;
	private GameObject[] soldiers;
	
	private int playersAcross = 0;
	
	private GUIText stateText;
	private GUIText scoreText;
	private RetryButton retry;
	
	private int currentNode;
	private int currentPath;
	private int nodesInPath;
	private GameObject[,] nodes;
	
	private int[] pathLengths;
	private int pathsDone = 0;
	
	private bool gameOver = false;
	
	// Use this for initialization
	void Start () {
		
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		soldiers = GameObject.FindGameObjectsWithTag ("Soldier");
		stateText = GameObject.FindGameObjectWithTag ("StateText").GetComponent<GUIText> ();
		scoreText = GameObject.FindGameObjectWithTag ("ScoreText").GetComponent<GUIText> ();
		retry = GameObject.FindGameObjectWithTag ("RetryButton").GetComponent<RetryButton> ();
		pb = GameObject.FindGameObjectWithTag ("ProgressBar").GetComponent<ProgressBar> ();
		StorePlayerStartPositions ();
		InitializeNodeArray ();
	}
	
	private void StorePlayerStartPositions()
	{
		playerStartPositions = new Vector3[totalPaths];
		playerStartScales = new Vector3[totalPaths];
		
		
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		
		for(int i = 0; i < players.Length; i++)
		{
			int index = players[i].GetComponent<Player>().path;
			playerStartPositions[index] = players[i].transform.position;
			playerStartScales[index] = players[i].transform.localScale;
		}
	}
	
	private void InitializeNodeArray()
	{
		//all the nodes
		GameObject[] linearArray = GameObject.FindGameObjectsWithTag ("Node");
		
		nodes = new GameObject[10,10];
		pathLengths = new int[10];
		
		//print ("TOTAL LENGTH PATH 0 " + nodes.GetLength (0));
		//print ("TOTAL LENGTH PATH 1 " + nodes.GetLength (1));
		
		
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

//	private void GetScore()
//	{
//		return playersAcross;
//	}
	
	public void SpawnPlayerAtPath (int pathNo)
	{
		Vector3 spawnPos = playerStartPositions [pathNo];

		GameObject newPlayer;
		
		//spawn off screen
		spawnPos.x += 10;
		if(pathNo == 0)
			newPlayer = GameObject.Instantiate (playerObjPath0, spawnPos, Quaternion.identity) as GameObject;
		else if(pathNo == 1)
			newPlayer = GameObject.Instantiate (playerObjPath1, spawnPos, Quaternion.identity) as GameObject;
		else if(pathNo == 2)
			newPlayer = GameObject.Instantiate (playerObjPath2, spawnPos, Quaternion.identity) as GameObject;
		else
		{
			print ("Invalid path");
			return;
		}

		newPlayer.transform.localScale = playerStartScales [pathNo];
		newPlayer.GetComponent<Player> ().path = pathNo;
		newPlayer.GetComponent<Player>().GetIn (playerStartPositions [pathNo]);
		print ("new spawned");
		
		ResetPath (pathNo);
		
	}
	
	private void ResetPath(int pathNo)
	{
		//lock all nodes 
		for(int i = 0; i < pathLengths[pathNo]; i++)
		{
			nodes[pathNo,i].GetComponent<ClickNode>().LockNode();
		}
		
		//unlock first node
		nodes[pathNo,0].GetComponent<ClickNode>().UnlockNode();
	}
	
	public void HitNodeNumber(int pathNo, int numHit)
	{
		print ("HIT NODE WITH PATH : " + pathNo + " AND NODE :" + numHit);
		if(numHit == pathLengths[pathNo]-1)
			PathDone(pathNo);
		//WinGame();
		else
		{

			for(int i = 0; i < pathLengths[pathNo]; i++)
			{
				//unlock the next node
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
		gameOver = true;
		stateText.text = "Time up!";
		scoreText.text = "Score: " + playersAcross;
		retry.Activate();
		foreach(GameObject g in soldiers)
			g.GetComponent<CreateVisionCone>().StopAllCoroutines();
		
	}
	
	public bool isGameOver()
	{
		return gameOver;
	}
	
	private void PathDone(int pathNo)
	{
		stateText.text = "Path "+pathNo+" completed!";
		playersAcross++;
		pb.UpdateScore (playersAcross);
		pathsDone++;
		print ("paths done : " + pathsDone);
		
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		
		//remove the player
		for(int i = 0; i < players.Length; i++)
		{
			if(players[i].GetComponent<Player>().path == pathNo)
				players[i].GetComponent<Player>().GetOut();
			//Destroy(players[i]);
		}
		
		SpawnPlayerAtPath (pathNo);
		
		//if(pathsDone == totalPaths)
		//WinGame();
		//else
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
		scoreText.text = "Score: " + playersAcross;
		retry.Activate();
	}
	
	public void RestartLevel()
	{
		Application.LoadLevel (0);
	}
}
