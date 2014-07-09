using UnityEngine;
using System.Collections;

public class apriBotola : MonoBehaviour {

	private GameObject player;
	public GameObject botola;
	public bool chiavePresa;
	// Use this for initialization

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (chiavePresa && Vector2.Distance(player.GetComponent<Transform>().position, transform.position) < 0.5 
		    && Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.X))
		{
			botola.GetComponent<botola>().open = true;
		}
	}
}
