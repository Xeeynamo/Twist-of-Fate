using UnityEngine;
using System.Collections;

public class TimerToBase : MonoBehaviour {

	public int maxTimer;
	private float timer;
	// Use this for initialization
	void Start () {
		timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (timer >= maxTimer)
			Application.LoadLevel (0);
		timer += Time.deltaTime;
	}
}
