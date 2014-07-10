using UnityEngine;
using System.Collections;

public class MostraDialogo : MonoBehaviour {

	private GameObject player; 
	public static string nomeGiocatore;
	public int numUccisioni;

	private string[] dialoghi;

	private bool inDialogue = false;

	public int areaWidth = 472;
	public int areaHeight = 74;

	private int actDialogue;
	private Texture2D dialogueBg;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");

		dialogueBg = new Texture2D (areaWidth, areaHeight );
		for (int i = 0; i < dialogueBg.width; i++)
		{
			for (int j = 0; j < dialogueBg.height; j++)
			{
				dialogueBg.SetPixel (i, j, new Color(0,0,0,0.5f));
			}
		}
		dialogueBg.Apply ();
		nomeGiocatore = PlayerPrefs.GetString ("Player_Name");
		dialoghi = new string[5];
		dialoghi[0] = "Prigioniero\nPuoi Sentirmi?\n- Premi X per continuare -";
		dialoghi[1] = nomeGiocatore +"\nCerto che posso. Chi sei?\n- Premi X per continuare -";
		dialoghi[2] = "Prigioniero\nSono il signore di questo castello, ti prego liberami.\nUn impostore ha preso il mio posto.\n- Premi X per continuare -";
		if (numUccisioni == 0)
		{
			dialoghi[3] = nomeGiocatore +"\nTi credo, appena riuscirò a fuggire da questo incubo tornerò a cercarti, fidati di me.\n- Premi X per continuare -";
			dialoghi[4] = "Prigioniero\nTi ringrazio, pregherò per te.\n- Premi X per continuare -";
		}
		else
		{
			dialoghi[3] = nomeGiocatore +"\nE chi mi dice che anche tu non sia un impostore?\n*Meglio non fidarsi*\n\n- Premi X per continuare -";
			dialoghi[4] = "Prigioniero\nTi prego Salvami.\n- Premi X per continuare -";
		}
		actDialogue = 0;
	}

	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.X) && inDialogue)
			actDialogue++;
		if (Mathf.Abs (Vector2.Distance (transform.position, player.GetComponent<Transform> ().position)) < 1 && Input.GetKeyDown (KeyCode.X))
		{
			inDialogue = true;
			Time.timeScale = 0;
		}
		if (actDialogue == dialoghi.Length)
		{
			Destroy (gameObject);
			Time.timeScale = 1;
		}
	}

	void OnGUI()
	{
		if(inDialogue)
		{
			float screenX = ((Screen.width * 0.55f) - (areaWidth * 0.5f)); 
			float screenY = ((Screen.height * 0.82f) - (areaHeight * 0.5f)); 
			
			GUILayout.BeginArea (new Rect(screenX, screenY, areaWidth, areaHeight), dialogueBg);
			GUILayout.Label (dialoghi[actDialogue]);
			GUILayout.EndArea();
		}
	}
}
