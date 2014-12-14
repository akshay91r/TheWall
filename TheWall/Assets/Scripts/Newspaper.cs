using UnityEngine;
using System.Collections;

public class Newspaper : MonoBehaviour {

	private string wall1Path = "Wall1/";
	private string wall2Path = "Wall2/";
	private string wall3Path = "Wall3/";
	private string wall4Path = "Wall4/";

	public void LoadRandomPage(int pLevel)
	{
		if(pLevel == 0)//wall1
		{
			int randomPage = Random.Range(0,13);
			GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(string.Concat(wall1Path,"clipping"+randomPage), typeof(Sprite));
		}
	}
}
