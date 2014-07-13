using UnityEngine;
using System.Collections;

public class FineLivello : MonoBehaviour {
	private Transform player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector2.Distance (player.position, transform.position) < 1)
			Application.LoadLevel (4);
	}
}
