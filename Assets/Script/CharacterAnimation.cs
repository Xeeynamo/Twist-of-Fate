using UnityEngine;
using System.Collections;

public class CharacterAnimation : MonoBehaviour {
	public CharacterAction.StatoInput animCorrente;
	// Use this for initialization
	protected Transform trans;
	private Animator anim;


	void Awake()
	{
		anim = this.GetComponent<Animator>();

	}

	void Start () {
		animCorrente = CharacterAction.StatoInput.Base;
		trans = transform;
	}
	bool Directon 
	{
		get { return trans.transform.rotation.y == 0; }
		set
		{
			Quaternion rot = trans.transform.rotation;
			rot.y = value ? 0 : 180;
			trans.transform.rotation = rot;
		}
	}
	public void setAnimation(CharacterAction.StatoInput stato){
		animCorrente = stato;
	}
	// Update is called once per frame
	void Update () {
		switch (animCorrente) {
		case CharacterAction.StatoInput.Base:		
			anim.SetInteger("Stato", 0);
			break;

		case CharacterAction.StatoInput.CamminaDx:
			anim.SetInteger("Stato", 1);
			Directon = true;
			break;

		case CharacterAction.StatoInput.CamminaSx:
			anim.SetInteger("Stato", 1);
			Directon = false;
			break;

		case CharacterAction.StatoInput.CorriDx:
			anim.SetInteger("Stato", 1);
			Directon = true;
			break;

		case CharacterAction.StatoInput.CorriSx:
			anim.SetInteger("Stato", 1);
			Directon = false;
			break;

		case CharacterAction.StatoInput.Attacco:
			
			break;

		case CharacterAction.StatoInput.Difesa:
			
			break;

		case CharacterAction.StatoInput.Abbassato:
			anim.SetInteger("Stato", 2);
			break;

		case CharacterAction.StatoInput.Arrampicata:
			
			break;

		case CharacterAction.StatoInput.Muro:
			
			break;
		}

	}
}
