using UnityEngine;
using System.Collections;

public class NextLevelButton : MonoBehaviour {
	
	private bool clicked = false;
	private bool hover = false;

	public Sprite InactiveSprite;
	public Sprite HoverSprite;
	public Sprite ClickSprite;

	private int numberOfLevels;

	void Start()
	{
		CalculateNumberOfLevels ();
	}

	void CalculateNumberOfLevels()
	{
		int count = 0;

		foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
		{
			numberOfLevels++;
		}
		print ("levels = " + numberOfLevels);
	}
	
	void OnMouseDown()
	{
		clicked = true;
		GetComponent<SpriteRenderer> ().sprite = ClickSprite;
	}

	void OnMouseOver()
	{
		if(!hover)
		{
			hover = true;
			GetComponent<SpriteRenderer> ().sprite = HoverSprite;
			transform.localScale *= 1.1f;
		}
	}

	void OnMouseExit()
	{
		if(hover)
		{
			hover = false;
			GetComponent<SpriteRenderer> ().sprite = InactiveSprite;
			transform.localScale /= 1.1f;
		}
	}
	
	void OnMouseUp()
	{
		clicked = false;
		GetComponent<SpriteRenderer> ().sprite = InactiveSprite;

		transform.parent.gameObject.GetComponent<LevelCompleteScreen> ().MoveOut ();

		//replace this when this button isn't drawn for the last level
		if((Application.loadedLevel+1) < numberOfLevels)
			Application.LoadLevel(Application.loadedLevel+1);
	}
}





