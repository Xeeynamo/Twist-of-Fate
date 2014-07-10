using UnityEngine;
using System.Collections;

public class CollisioniTrappoleSpine : MonoBehaviour {
	private int playerMask = (1 << 13);
	private Color c;
	private bool exe = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Physics2D.Raycast (new Vector2 (this.transform.position.x + 0.2f, this.transform.position.y + 0.08f), Vector3.right, 0.65f, playerMask)) {


			c = Color.red;
			exe = true;
		} else {
			c = Color.grey;

			if(exe){
	
				exe = false;
			}
		}
		Debug.DrawRay(new Vector2 (this.transform.position.x+0.2f, this.transform.position.y + 0.08f), Vector3.right * 0.65f, c);
	}
	}

