using UnityEngine;
using System.Collections;

public class MuoviProiettile : MonoBehaviour
{

    public float Velocità;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		this.transform.position += new Vector3 (Velocità, 0, 0) * Time.deltaTime;

		if (this.transform.position.x < -2.384186e-07 || this.transform.position.x >19.2)
						Destroy (this.gameObject);

	
}

 void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
						audio.Play ();

						//Destroy (this.gameObject);
				}
		//applica danno
	}

}