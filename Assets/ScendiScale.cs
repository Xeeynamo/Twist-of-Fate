using UnityEngine;
using System.Collections;

public class ScendiScale : MonoBehaviour {
	public static bool scaleGiù = false;
	private int playerMask = 1 << 13;
	private Color c;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Physics2D.Raycast (new Vector2 (this.transform.position.x - 0.1f, this.transform.position.y +0.01f), Vector3.right, 0.50f, playerMask)) {
			c = Color.red;		
			if(We.Input.MoveDown){
				scaleGiù = true;
				c = Color.white;
			}
			else
			{
				scaleGiù = false;
			}
		}
		else 
			c = Color.blue;
		
		Debug.DrawRay(new Vector2 (this.transform.position.x - 0.1f, this.transform.position.y + 0.01f),Vector3.right * 0.50f, c);
	}
}
