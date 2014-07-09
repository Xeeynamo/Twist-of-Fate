using UnityEngine;
using System.Collections;

public class ZonaCellaRotta : MonoBehaviour {
	private int playerMask = 1 << 13;
	private Color c;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Physics2D.Raycast (new Vector2 (this.transform.position.x + 0.2f, this.transform.position.y + 0.08f), Vector3.right, 0.65f, playerMask)) {
						print ("Qui può nascondersi");
						c = Color.red;
				} else {
						c = Color.grey;
			print ("Qui non può nascondersi");
				}
		Debug.DrawRay(new Vector2 (this.transform.position.x+0.2f, this.transform.position.y + 0.08f), Vector3.right * 0.65f, c);
	}
}
