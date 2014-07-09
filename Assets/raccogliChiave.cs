using UnityEngine;
using System.Collections;

public class raccogliChiave : MonoBehaviour {

	private GameObject player;
	public GameObject serratura;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if(Vector2.Distance(player.GetComponent<Transform>().position, transform.position) < 0.5 && Input.GetKeyDown(KeyCode.X))
		{
			serratura.GetComponent<apriBotola>().chiavePresa = true;
			Destroy(gameObject);
		}
	}
}
