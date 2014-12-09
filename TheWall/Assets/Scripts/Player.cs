using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed = 7.5f;
	private bool moving = false;

	private Blackboard bb;

	public bool alive = true;
	private Sprite playerSprite;

	public Sprite human;
	public Sprite redHuman;

	// Use this for initialization
	void Start () {

		bb = GameObject.FindGameObjectWithTag ("Blackboard").GetComponent<Blackboard> ();

		playerSprite = GetComponent<SpriteRenderer> ().sprite;
		playerSprite = redHuman;
	
	}

	public void MoveToPosition(Vector3 pos)
	{
		if(moving)
		{
			StopAllCoroutines();
			moving = false;
			return;
		}

		moving = true;
		StartCoroutine(Move (pos));
	}

	private IEnumerator Move(Vector3 pos){
		//
		
		while(Vector3.Distance(transform.position, pos) > 0.05f)
		{
			transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
			
			yield return null;
		}

		moving = false;
		yield return new WaitForSeconds(0f);

		bb.WinGame ();
	}

	public void Die()
	{
		GetComponent<SpriteRenderer> ().sprite = redHuman;
		StopAllCoroutines ();
	}
}
