using UnityEngine;
using System.Collections;

public class MainMenuBtn : MonoBehaviour {

	public string levelToLoad;
	public Texture2D normalTexture;
	public Texture2D rollOverTexture;
	public AudioClip beep;
	public bool quitButton = false;
	public bool comandi = false;

	private AudioSource music;
	// Use this for initialization
	void Start () {
		music = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter()
	{
		guiTexture.texture = rollOverTexture;
	}

	void OnMouseExit()
	{
		guiTexture.texture = normalTexture;
	}

	IEnumerator OnMouseUp()
	{
		audio.PlayOneShot (beep);
		yield return new WaitForSeconds(1f);
		if (quitButton)
			Application.Quit ();
		else if (comandi)
			Application.LoadLevel (5);
		else
			Application.LoadLevel (1);
	}
}
