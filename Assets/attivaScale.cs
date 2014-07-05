using UnityEngine;
using System.Collections;

public class attivaScale : MonoBehaviour {
	EdgeCollider2D att;
	// Use this for initialization
	void Start () {
		att = GetComponent<EdgeCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		att.enabled= (SaliScale.scaleSu);
	}
}
