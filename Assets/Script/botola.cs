using UnityEngine;
using System.Collections;

public class botola : MonoBehaviour {

	public bool open;
	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(open)
		{
			anim.SetBool("Apri" ,true);
			open = false;
		}
		if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Base"))
			anim.SetBool("Apri" ,false);
	}
}
