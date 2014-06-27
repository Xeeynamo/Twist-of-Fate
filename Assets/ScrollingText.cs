using UnityEngine;
using System.Collections;

public class ScrollingText : MonoBehaviour {
	
	public int caratteriAlSecondo = 8;
	public string[] showText;

	private float secondiPerOgniCarattere;
	private float timer = 0f;
	private int curChar;
	private int actText;

	private GUIText testo;

	// Use this for initialization
	void Start () { 
		testo = GetComponent<GUIText>();
		testo.text = "";
		curChar = 0;
		secondiPerOgniCarattere = 1.0f/caratteriAlSecondo;
		actText = 0;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if(timer > secondiPerOgniCarattere && curChar < showText[actText].Length)
		{
			int actChar = Mathf.FloorToInt(timer/secondiPerOgniCarattere);
			curChar += actChar;
			if(curChar > showText[actText].Length)
				curChar = showText[actText].Length;
			timer -= actChar * secondiPerOgniCarattere;
			testo.text = showText[actText].Substring(0, curChar);
		}
		if(Input.GetKeyDown(KeyCode.X))
		{
			timer = 0f;
			testo.text = "";
			actText++;
			curChar = 0;
		}
	}
}
