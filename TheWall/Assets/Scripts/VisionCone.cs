using UnityEngine;
using System.Collections;

public class VisionCone : MonoBehaviour {

	private Blackboard bb;
	private SoundManager sm;

	// Use this for initialization
	void Start () {
	
		bb = GameObject.FindGameObjectWithTag ("Blackboard").GetComponent<Blackboard> ();
		sm = GameObject.FindGameObjectWithTag ("SoundManager").GetComponent<SoundManager> ();
	}

	void OnTriggerEnter(Collider col) 
	{
		if(col.gameObject.tag == "Player")
		{
			CreateVisionCone create = transform.parent.transform.parent.gameObject.GetComponent<CreateVisionCone>();

			if(create.IsDog)
				sm.PlayFoleySound(5);
			else
				sm.PlayFoleySound(6);

			create.ShowGuardAlert();
			col.gameObject.GetComponent<Player>().Die ();
		}
	}
}
