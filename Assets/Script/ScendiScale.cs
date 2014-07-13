using UnityEngine;
using System.Collections;

public class ScendiScale : MonoBehaviour {
	public static bool scaleGiù = false;
	private int playerMask = 1 << 13;
	private Color c;
	public bool lato;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Physics2D.Raycast (new Vector2 (this.transform.position.x , this.transform.position.y +0.08f),lato? Vector3.right : Vector3.left, 0.50f, playerMask)) {
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
		
		Debug.DrawRay(new Vector2 (this.transform.position.x, this.transform.position.y + 0.08f),lato? Vector3.right * 0.50f : Vector3.left * 0.50f, c);

	}
}
