using UnityEngine;
using System.Collections;

public class PlayAgainButton : MonoBehaviour {

	private bool clicked = false;
	private bool hover = false;

	
	public Sprite InactiveSprite;
	public Sprite HoverSprite;
	public Sprite ClickSprite;
	
	private int numberOfLevels;
	
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
		transform.parent.gameObject.GetComponent<LevelCompleteScreen> ().MoveOutAndRestart();
	}
}
