﻿using UnityEngine;
using System.Collections;

public class Arma : MonoBehaviour {
	public bool armaAttiva = false;
	bool colpo = false;
	int dannoArma = 10;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {


		if (armaAttiva) {
			RaycastHit2D oggettoColpito = Physics2D.Raycast (new Vector2 (this.transform.position.x + (Richard.physManager.Direction ? 0.3f:-0.3f) , this.transform.position.y), Richard.physManager.Direction ? Vector3.left * 0.5f : Vector3.right * 0.5f, PhysicsManager.ENEMY_LAYER);
			Debug.DrawRay (new Vector2 (this.transform.position.x + (Richard.physManager.Direction ? 0.3f:-0.3f), this.transform.position.y), Richard.physManager.Direction ? Vector3.left * 0.5f : Vector3.right * 0.5f, Color.red);
			if (oggettoColpito.collider.gameObject.tag == "Enemy"||oggettoColpito.collider.gameObject.tag == "Boss") {
								if (!colpo) {
										oggettoColpito.collider.gameObject.GetComponent<PhysicsManager> ().Health -= dannoArma;
					                    print (oggettoColpito.collider.gameObject.GetComponent<PhysicsManager> ().Health);
					                    oggettoColpito.collider.gameObject.audio.Play(); 
										colpo = true;					                    
								}
						}

				} else
						colpo = false;

	}
}
