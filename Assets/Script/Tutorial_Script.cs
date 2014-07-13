using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial_Script : MonoBehaviour {

	public Transform[] childTransform;
	private Transform playerTrans;
	
	public int areaWidth = 378;
	public int areaHeight = 196;

	private ArrayList activations;
	private ArrayList msgActivated;

	private float timer;

	private Dictionary<Transform, string> tutorialString;

	private Texture2D tutorialBg;

	public float posX, posY;

	public float time;

	// Use this for initialization
	void Start () {
		childTransform = GetComponentsInChildren<Transform>();
		playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		activations = new ArrayList ();
		msgActivated = new ArrayList ();
		timer = time;

		tutorialBg = new Texture2D (areaWidth, areaHeight );
		for (int i = 0; i < tutorialBg.width; i++)
		{
			for (int j = 0; j < tutorialBg.height; j++)
			{
				tutorialBg.SetPixel (i, j, new Color(0,0,0,0.5f));
			}
		}
		tutorialBg.Apply ();

		tutorialString = new Dictionary<Transform, string>();
		tutorialString.Add (childTransform[1], "Tutorial (Comandi Base 1)\nPremi Freccia Dx per muoverti a Destra\nPremi Freccia Sx per muoverti a Sinistra");
		tutorialString.Add (childTransform[2], "Tutorial (Comandi Base 2)\nPremi due volte in rapida successione Freccia Dx o Freccia Sx per correre in quella direzione. ");
		tutorialString.Add (childTransform[3], "Tutorial (Comandi Base 3)\nPremi Freccia Su per entrare in una Cella\nPremi \"V\" per Attaccare");
		tutorialString.Add (childTransform[4], "Tutorial (Comandi Base 4)\nPer salire sulle scale cammina tenendo premuto Frecca Su.");
		tutorialString.Add (childTransform[6], "Tutorial (Interazioni)\nPremere \"X\" per interagire con gli oggetti.");
		tutorialString.Add (childTransform[5], "Tutorial (Comandi Base 5)\nPremere Barra Spaziatrice per saltare.");
	}

	// Update is called once per frame
	void Update () {
		foreach (Transform t in childTransform)
		{
			if(!msgActivated.Contains(t))
			{
				if(Vector2.Distance (playerTrans.position, t.position) <= 1)
				{
					msgActivated.Add(t);
					activations.Add(t);
				}
			}
		}
	}
	
	void OnGUI()
	{
		if(timer > 0 && activations.Count > 0)
		{
			timer -= Time.deltaTime;
			float screenX = ((Screen.width * posX) - (areaWidth * 0.5f)); 
			float screenY = ((Screen.height * posY) - (areaHeight * 0.5f)); 

			GUILayout.BeginArea (new Rect(screenX, screenY, areaWidth, areaHeight), tutorialBg);
			GUILayout.Label (tutorialString[(Transform)activations[0]]);
			GUILayout.EndArea();
		}
		if (timer <= 0 && activations.Count > 0)
		{
			timer = time;

			activations.RemoveAt(0);
		}
	}
}
