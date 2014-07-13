using UnityEngine;
using System.Collections;

public class esci : MonoBehaviour {

	public bool exit;
	
	private Texture2D exitBg;
	
	private int areaWidth = 200;
	private int areaHeight = 50;
	
	public GUIStyle style;	
	// Use this for initialization
	void Start () {
		exit = false;
		exitBg = new Texture2D (areaWidth, areaHeight);
		for(int i = 0; i < exitBg.width; i++)
		{
			for (int j = 0; j < exitBg.height; j++)
				exitBg.SetPixel(i,j, new Color(0 , 0, 0, 0.5f));
		}
		exitBg.Apply ();
		style.fontSize = 14;
		style.normal.textColor = Color.white;
		style.alignment = TextAnchor.UpperCenter;
	}
	
	// Update is called once per frame
	void Update () {
		if(We.Input.Exit)
		{
			exit = !exit;
			if(exit)
			{
				Time.timeScale = 0;
				GetComponent<AudioSource>().Pause();
			}
			else
			{
				Time.timeScale = 1;
				GetComponent<AudioSource>().Play();
			}
		}
	}
	
	void OnGUI(){
		if (exit)
		{
			float screenX = ((Screen.width * 0.5f) - (areaWidth * 0.5f)); 
			float screenY = ((Screen.height * 0.5f) - (areaHeight * 0.5f)); 
			
			GUILayout.BeginArea (new Rect(screenX, screenY, areaWidth, areaHeight), exitBg);
			GUILayout.Label ("Sei sicuro di voler uscire?", style);
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("Si"))
				Application.LoadLevel(0);
			if(GUILayout.Button("No"))
			{
				exit= false;
				Time.timeScale = 1;
				GetComponent<AudioSource>().Play();
			}
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
	}

}
