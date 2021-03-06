﻿using UnityEngine;
using System.Collections;

public class StartGameButton : MonoBehaviour {

	private bool clicked = false;
	private bool hover = false;
	
	public Sprite InactiveSprite;
	public Sprite HoverSprite;
	public Sprite ClickSprite;

	void Start()
	{
		CheckForMultipleSoundManagers ();
	}

	void CheckForMultipleSoundManagers ()
	{
		GameObject[] sms = GameObject.FindGameObjectsWithTag ("SoundManager");

		if(sms.Length > 1)
		{
			//destroy extra one
			GameObject delete = GameObject.FindGameObjectWithTag ("SoundManager");
			Destroy(delete);
		}
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

		//load first level
		Application.LoadLevel(Application.loadedLevel+1);
	}
}
