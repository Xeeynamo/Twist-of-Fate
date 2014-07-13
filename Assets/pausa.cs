using UnityEngine;
using System.Collections;

public class pausa : MonoBehaviour {

	public bool pause;
	
	private Texture2D pauseBg;

	public float areaWidth;
	public float areaHeight;

	public GUIStyle style;	
	// Use this for initialization
	void Start () {
		pause = false;
		pauseBg = new Texture2D (Screen.width, Screen.height);
		for(int i = 0; i < pauseBg.width; i++)
		{
			for (int j = 0; j < pauseBg.height; j++)
				pauseBg.SetPixel(i,j, new Color(0 , 0, 0, 0.5f));
		}
		pauseBg.Apply ();
	}
	
	// Update is called once per frame
	void Update () {
		if(We.Input.Pause)
		{
			pause = !pause;
			if(pause)
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
		if (pause)
		{
			GUILayout.BeginArea (new Rect(0, 0, pauseBg.width, pauseBg.height), pauseBg);
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label( "Pausa" , style);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.EndArea();
		}
	}

}
