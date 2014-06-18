using UnityEngine;
using System.Collections;

public class CharacterAction : MonoBehaviour {
	//Tipo enumerativo dei possibili stati del personaggio
	public enum StatoInput{
		Base,
		CamminaDx,
		CamminaSx,
		CorriDx,
		CorriSx,
		Salta,
		Abbassato,
		Muro,
		Arrampicata,
		Attacco,
		Difesa

	}

	//Variabile che contiene lo stato dell'input attuale
	 public StatoInput statoCorrente = StatoInput.Base;

	//Tipo enumerativo delledirezioni che può assumere il personaggio
	[HideInInspector] public enum Direzione { DX, Sx }
	[HideInInspector] public Direzione direzionePg;

	//Variabile per la trasformazione grafica della sprite del personaggio
	protected Transform trans;
	//Variabile rigidbody delle proprietà fisiche del personaggio
	protected Rigidbody2D rgbody;

	//Variabili di gestione della fisica del personaggio, possono essere cambiate per cambiare le caratteristiche del movimento
	private float camminata = 2f;
	private float corsa = 3f;
	private float forzaSalto = 10f;
	private float gravità = 50f;
	
	//Variabile contenente la velocità ATTUALE assunta dal personaggio
	public Vector2 fisVel = new Vector2();
	//controlla se il personaggio collide o meno con il terreno
	public bool terra = false;
	public bool salto = false;
	private int groundMask = 1 << 8; // Ground layer mask
	private float lung = 0.45f; //Lunghezza raycast 

	public virtual void Awake()
	{
		trans = transform;
		rgbody = rigidbody2D;
	}

	// Use this for initialization
	void Start () {

	}
	
	// FixedUpdate is called once per frame
	void FixedUpdate () {
		if (statoCorrente == StatoInput.Base) {
			//è fermo, annulla tutte le forze
			
			fisVel = Vector2.zero;
		}

		if (statoCorrente == StatoInput.CamminaDx) {
		    //rivolto verso destra
			//richiama l'animazione della camminata
			//modifica la velocità di spostamento

			fisVel.x=camminata;
		}

		if (statoCorrente == StatoInput.CamminaSx) {
			//rivolto verso sinistra
			//richiama l'animazione della camminata
			//modifica la velocità di spostamento
			fisVel.x=-camminata;

		}
		if (statoCorrente == StatoInput.CorriDx) {
			//rivolto verso destra
			//richiama l'animazione della corsa
			//modifica la velocità di spostamento
			fisVel.x=corsa;
		}
		
		if (statoCorrente == StatoInput.CorriSx) {
			//rivolto verso sinistra
			//richiama l'animazione della corsa
			//modifica la velocità di spostamento
			fisVel.x=-corsa;
		}

		if (statoCorrente == StatoInput.Salta) {
			if(!salto){
				fisVel.y=forzaSalto;

				salto = true;
			}
		}

		if (statoCorrente == StatoInput.Abbassato) {

		}

		if (statoCorrente == StatoInput.Muro) {
			
		}

		if (statoCorrente == StatoInput.Arrampicata) {
			
		}

		if (statoCorrente == StatoInput.Attacco) {
			
		}

		if (statoCorrente == StatoInput.Difesa) {
			
		}

		//Gestione gravità e terreno///////////////////////////////////////////////////////////////////////////////
		if (Physics2D.Raycast(new Vector2(trans.position.x-0.1f,trans.position.y), Vector3.down, lung, groundMask)||Physics2D.Raycast(new Vector2(trans.position.x+0.1f,trans.position.y), Vector3.down, lung, groundMask))
		{
			terra = true;
			salto = false;
		}
		else
		{
			terra = false;
			rgbody.AddForce(Vector3.down * gravità);
		}
		Color col = Color.green;
		if (terra)
		col = Color.red;
		Debug.DrawRay (new Vector2(trans.position.x-0.1f,trans.position.y), Vector3.down*lung,col);
		Debug.DrawRay (new Vector2(trans.position.x+0.1f,trans.position.y), Vector3.down*lung,col);
		////////////////////////////////////////////////////////////////////////////////////////////////////////////
		rgbody.AddForce (new Vector2(fisVel.x*100,fisVel.y*100));

		setStato ();
	}

	public void setStato(){
		 if ((We.Input.MoveRight == true) && (We.Input.Attack == true))
			statoCorrente = StatoInput.CorriDx;

		else if ((We.Input.MoveLeft == true) && (We.Input.Attack == true))
			statoCorrente = StatoInput.CorriSx;

		else if (We.Input.MoveRight == true)
			statoCorrente = StatoInput.CamminaDx;	

		else if (We.Input.MoveLeft == true)	
			statoCorrente = StatoInput.CamminaSx;
			
		else
			statoCorrente = StatoInput.Base;
		
		if (We.Input.Jump)
			statoCorrente = StatoInput.Salta;	


	}
}
