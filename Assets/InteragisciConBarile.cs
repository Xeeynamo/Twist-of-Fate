using UnityEngine;
using System.Collections;

public class InteragisciConBarile : MonoBehaviour {

	public int aumentoVita = 30;
	public int numUsage = 3;
	private int playerMask = 1 << 13;
	private Color c;

	public float timerNextUsage = 5f;
	private float timer = 0;

	private SpriteRenderer sr;
	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Physics2D.Raycast (new Vector2 (this.transform.position.x, this.transform.position.y),Vector3.right, 0.50f, playerMask)) 
		{
			c = Color.red;		
			if(We.Input.Defense && numUsage > 0 && timer <= 0){
				GameObject.FindGameObjectWithTag("Player").GetComponent<PhysicsManager>().Health += aumentoVita;
				c = Color.white;
				numUsage--;
				timer = timerNextUsage;
			}
		}

		if (timer > 0)
		{
			timer -= Time.deltaTime;
		}

		Debug.DrawRay(new Vector2 (this.transform.position.x - sr.sprite.bounds.center.x, this.transform.position.y),Vector3.right , c);
		
	}
}
