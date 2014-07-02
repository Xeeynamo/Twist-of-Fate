using UnityEngine;
using System.Collections;

public class MuoviProiettile : MonoBehaviour {
	private float movimento = 500; // vellocità di spostamento del proiettile

    public float Velocità;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		this.transform.position += new Vector3 (Velocità, 0, 0) * Time.deltaTime;
}
}