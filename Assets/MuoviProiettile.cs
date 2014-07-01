using UnityEngine;
using System.Collections;

public class MuoviProiettile : MonoBehaviour {
	private float movimento = 500; // vellocità di spostamento del proiettile
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		this.transform.position += new Vector3 (-4, 0, 0) * Time.deltaTime;
}
}