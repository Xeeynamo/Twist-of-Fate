using UnityEngine;
using System.Collections;

public class FadeInOut : MonoBehaviour {
	public bool fadeIn = false;
	public bool fadeOut = false;
	private GUIText testo;

	Color endWhite = new Color(0.8f, 0.8f, 0.8f);
	Color endBlack = new Color(0.1f, 0.1f, 0.1f);

	// Use this for initialization
	void Start () {
		testo = GetComponent<GUIText> ();
		print (testo.color);
	}
	
	// Update is called once per frame
	void Update () {
		if(fadeIn)
			testo.color = Color.Lerp (testo.color, Color.white, 1 * Time.deltaTime);
		if(fadeOut)
			testo.color = Color.Lerp (testo.color, Color.clear, 4 * Time.deltaTime);
		if (fadeIn && testo.color == endWhite)
		{
			fadeIn = false;
			testo.color = Color.white;
		}

	}
}
