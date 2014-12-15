using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	
	private GameObject BGMusic;
	private GameObject foley;
	
	//for instantiating foley sounds
	public GameObject newSound;

	private string musicPath = "Sounds/BGMusic/";
	private string foleyPath = "Sounds/Foley/";

	//Foley sounds
	private string[] foleySounds = new string[] {"click", "defectorDrowning", "defectorDying", "defectorDying2", "defectorDying3", "dogBarking", "gunShotWithReload", "saveAGuy"};


	void Awake(){
		BGMusic = transform.Find ("BGMusic").gameObject;
		DontDestroyOnLoad (gameObject);
	}
	
//	public void PlayMusic()
//	{
//		AudioClip newClip = (AudioClip)Resources.Load(string.Concat(musicPath,"BGTrack"), typeof(AudioClip));
//		BGMusic.GetComponent<AudioSource> ().clip = newClip;
//		BGMusic.GetComponent<AudioSource> ().Play ();
//	}
	
	public void PlayFoleySound(int num)
	{
		AudioClip newClip = (AudioClip)Resources.Load(string.Concat(foleyPath,foleySounds[num]), typeof(AudioClip));
		
		//trying new logic, spawning a gameobject for the sound
		GameObject newFoley = (GameObject)Instantiate (newSound);
		newFoley.transform.parent = transform;
		
		newFoley.GetComponent<AudioSource> ().clip = newClip;
		StartCoroutine(PlayAndDestroy (newFoley, newClip.length));
	}

	public void PlayDefectorDyingSound()
	{
		int randomSound = Random.Range (2, 5);
		PlayFoleySound (randomSound);
	}

	private IEnumerator PlayAndDestroy(GameObject newFoley, float pLength)
	{
		newFoley.GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds(pLength);
		Destroy (newFoley);
	}
}
