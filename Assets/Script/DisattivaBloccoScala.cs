using UnityEngine;
using System.Collections;

public class DisattivaBloccoScala : MonoBehaviour {
	private BoxCollider2D blocco;
	//1 scala orientata verso sinistra, 0 scala orientata verso destra
	public float direzione;
	// Use this for initialization
	void Start () {
		blocco = GetComponent <BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		//controlla se il tasto per salire è stato premuto e se le direzioni tra scala e personaggio sono opposte
		 blocco.enabled = (CharacterAction.tastoScale && (CharacterAction.trans.rotation.y == direzione) );
	}
}
