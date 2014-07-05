using UnityEngine;
using System.Collections;

public class SaliScale : MonoBehaviour {
	public static bool scaleSu = false;
	private int playerMask = 1 << 13;
	private Color c;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Physics2D.Raycast (new Vector2 (this.transform.position.x + 0.1f, this.transform.position.y +0.01f), Vector3.right, 0.50f, playerMask)) {
			c = Color.red;		
			if(We.Input.MoveUp){
				scaleSu = true;
				c = Color.white;
			}
			else
			{
				scaleSu = false;
			}
		}
		else 
			c = Color.blue;

		Debug.DrawRay(new Vector2 (this.transform.position.x + 0.1f, this.transform.position.y + 0.01f),Vector3.right * 0.50f, c);
	}
}
