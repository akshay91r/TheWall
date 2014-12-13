using UnityEngine;
using System.Collections;

public class ProgressBar : MonoBehaviour {

	//private Blackboard bb;
	private float barDisplay = 0; //current progress
	public Vector2 pos = new Vector2(20,40);
	private Vector2 size = new Vector2(400,200);

	private Texture2D bronzeBar;
	private Texture2D silverBar;
	private Texture2D goldBar;
	private Texture2D emptyTex;
	private Texture2D fullTex;

	private Texture2D bronzeTrophy;
	private Texture2D silverTrophy;
	private Texture2D goldTrophy;

	public float speed = 0.05f;
	private float TimetoFillUp = 10;
//	float native_width = 1920;
//	float native_height = 1080;

	float native_width = Screen.width*3;
	float native_height = Screen.height*4;

	public float scoreForBronze = 2;
	public float scoreForSilver = 4;
	public float scoreForGold = 6;

	private int score = 0;

	int achievement = 0;

	GUIStyle customStyle;

	void Start()
	{
		bronzeBar = (Texture2D)Resources.Load(("ProgressBar/BronzeBar"), typeof(Texture2D));
		silverBar = (Texture2D)Resources.Load(("ProgressBar/SilverBar"), typeof(Texture2D));
		goldBar = (Texture2D)Resources.Load(("ProgressBar/GoldBar"), typeof(Texture2D));

		emptyTex = (Texture2D)Resources.Load(("ProgressBar/WhiteBar"), typeof(Texture2D));
		fullTex = (Texture2D)Resources.Load(("ProgressBar/BlackBar"), typeof(Texture2D));

		bronzeTrophy = (Texture2D)Resources.Load(("ProgressBar/BronzeTrophy"), typeof(Texture2D));
		silverTrophy = (Texture2D)Resources.Load(("ProgressBar/SilverTrophy"), typeof(Texture2D));
		goldTrophy = (Texture2D)Resources.Load(("ProgressBar/GoldTrophy"), typeof(Texture2D));


		//speed = 1 / TimetoFillUp;
	}

	void OnGUI() {
		//draw the background:

		float rx = Screen.width / native_width;
		float ry = Screen.height / native_height;
		GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (rx, ry, 1));


		GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
		GUI.Box(new Rect(0,0, size.x, size.y), fullTex);

		//bronze trophy
		GUI.Box(new Rect(size.x*0.15f,size.y/4, size.x/2, size.y/2), bronzeTrophy, GUI.skin.label);
		//silver trophy
		GUI.Box(new Rect(size.x*0.45f,size.y/4, size.x/2, size.y/2), silverTrophy, GUI.skin.label);
		//gold trophy
		GUI.Box(new Rect(size.x*0.75f,size.y/4, size.x/2, size.y/2), goldTrophy, GUI.skin.label);
		
		//draw the filled-in part:
		GUI.BeginGroup(new Rect(0,0, size.x * barDisplay, size.y));

		if(achievement == 0)
			GUI.Box(new Rect(0,0, size.x, size.y), emptyTex);
		else if(achievement == 1)
			GUI.Box(new Rect(0,0, size.x, size.y), bronzeBar);
		else if(achievement == 2)
			GUI.Box(new Rect(0,0, size.x, size.y), silverBar);
		if(achievement == 3)
			GUI.Box(new Rect(0,0, size.x, size.y), goldBar);

		//bronze trophy
		GUI.Box(new Rect(size.x*0.15f,size.y/4, size.x/2, size.y/2), bronzeTrophy, GUI.skin.label);
		//silver trophy
		GUI.Box(new Rect(size.x*0.45f,size.y/4, size.x/2, size.y/2), silverTrophy, GUI.skin.label);
		//gold trophy
		GUI.Box(new Rect(size.x*0.75f,size.y/4, size.x/2, size.y/2), goldTrophy, GUI.skin.label);

		GUI.EndGroup();
		GUI.EndGroup();
	}

	public void UpdateScore(int pScore)
	{
		score = pScore;
		float amountToFill = pScore / scoreForGold; //since gold is when the whole bar is filled
		StartCoroutine(DrawBar (amountToFill));
	}

	private IEnumerator DrawBar(float targetAmt)
	{
		while(barDisplay <= targetAmt && barDisplay <= 1)
		{
			barDisplay += Time.deltaTime*speed;

			yield return null;
		}

		yield return new WaitForSeconds(0f);

		if(score >= scoreForBronze && score < scoreForSilver)
			achievement = 1;
		else if(score >= scoreForSilver && score < scoreForGold)
			achievement = 2;
		else if(score >= scoreForGold)
			achievement = 3;
	}

//	void ChangeColor()
//	{
//		achievement++;
//		if(achievement > 2)
//			achievement = 0;
//
//		Invoke ("ChangeColor", TimetoFillUp/3);
//	}

	void SetAchievement(int pState)
	{
		achievement = pState;
	}
}
