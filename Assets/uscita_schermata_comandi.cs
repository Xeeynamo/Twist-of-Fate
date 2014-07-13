using UnityEngine;
using System.Collections;

public class uscita_schermata_comandi : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (We.Input.Exit)
						Application.LoadLevel (0);
	}
}
