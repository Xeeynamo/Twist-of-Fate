using UnityEngine;
using System.Collections;

public class Activation_Script : MonoBehaviour {

	private Transform playerPos;
	private Animator anim;
	// Use this for initialization
	void Start () {
		playerPos = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
		anim = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if ((Vector2.Distance (playerPos.position, transform.position) <= 1) && Input.GetKeyDown(KeyCode.X))
			anim.SetBool ("Open", true);
		if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Base"))
			anim.SetBool ("Open", false);
	}
}
