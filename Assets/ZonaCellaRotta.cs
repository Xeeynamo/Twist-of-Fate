using UnityEngine;
using System.Collections;

public class ZonaCellaRotta : MonoBehaviour {
	private int playerMask = 1 << 13;
	private Color c;
	//Exe sta per execute, permette di far si che il settaggio del flag sia fatto solo una volta così da non interferire con gli altri
	private bool exe = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Physics2D.Raycast (new Vector2 (this.transform.position.x + 0.2f, this.transform.position.y + 0.08f), Vector3.right, 0.65f, playerMask)) {
						print ("Qui può nascondersi");
			            Richard.canHide = true;
						c = Color.red;
			            exe = true;
				} else {
						c = Color.grey;
			print ("Qui non può nascondersi");
			if(exe){
			  Richard.canHide = false;
				exe = false;
			}
				}
		Debug.DrawRay(new Vector2 (this.transform.position.x+0.2f, this.transform.position.y + 0.08f), Vector3.right * 0.65f, c);
	}
}
