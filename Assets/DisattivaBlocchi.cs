using UnityEngine;
using System.Collections;

public class DisattivaBlocchi : MonoBehaviour {
	private BoxCollider2D blocco;
	// Use this for initialization
	void Start () {
		blocco = GetComponent <BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		blocco.enabled = !CharacterAction.scale;
	}
}
