using UnityEngine;
using System.Collections;

public class char_contr : MonoBehaviour {

	/// <summary>
	/// Dice se il personaggio è in aria o meno
	/// </summary>
	public bool isOnGround = false;

	/// <summary>
	/// Velocità Y del personaggio
	/// </summary>
	public float speedY = 0.0f;

	/// <summary>
	/// Accelerazione del movimento orizzontale del personaggio
	/// </summary>
	public float acceleration;
	
	/// <summary>
	/// Forza del salto
	/// </summary>
	public float jump = 65000.0f;

	/// <summary>
	/// Forza di decremento del salto
	/// </summary>
	public float jumpDecrement = 5000.0f;
	
	/// <value>L'accelerazione da impostare</value>
	public float Acceleration
	{
		get { return acceleration; }
		set { acceleration = value; }
	}
	
	/// <value>Forza del salto da impostare
	public float Jump
	{
		get { return jump; }
		set { jump = value; }
	}


	private Animator anim;
	private Transform play;
	private BoxCollider2D box;
	private bool dx;
	private bool sx;

	private float timewait;


	/// <summary>
	/// true = guarda in avanti
	/// false = si gira
	/// </summary>
	/// <value><c>true</c> if directon; otherwise, <c>false</c>.</value>
	bool Directon 
	{
		get { return play.transform.rotation.y == 0; }
		set
		{
			Quaternion rot = play.transform.rotation;
			rot.y = value ? 0 : 180;
			play.transform.rotation = rot;
		}
	}

	void Awake()
	{
		anim = GameObject.FindGameObjectWithTag ("Player").GetComponent<Animator> ();
		play = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
		box = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
	}

	// Update is called once per frame
	void FixedUpdate ()
	{

		float comy = 0.0f;

		if (We.Input.MoveRight == true)
						DX ();
				else if (We.Input.MoveLeft == true)		
						SX ();
				else if (We.Input.MoveDown == true)		
						DWN ();	

		else
		{
			anim.SetInteger("Stato", 0);
//			float cazz = box.size.y;
			//cazz = 41f;
		}

		if (We.Input.Jump)
		{
			if (isOnGround == true)
			{
				isOnGround = false;
				speedY = Jump;
			}
		}

		if (isOnGround == false)
		{
			if (speedY > 0)
			{
				speedY -= jumpDecrement;
				if (speedY < 0)
					speedY = 0;
			}
			rigidbody2D.AddForce(new Vector2(0, speedY));
		}

		if (comy < 0)
		{
			anim.SetInteger("Stato", 2);
		}

		if (anim.GetCurrentAnimatorStateInfo (0).IsName("girata"))
		{
			anim.SetBool ("Gira", false);
		}



	}

	void Update()
	{
		if (anim.GetCurrentAnimatorStateInfo (0).IsName("Fermo")&& timewait!=0)
		{
			timewait = 0;
			dx = false;
			sx = false;
			anim.SetBool ("abbass", true);
		}
	}

	void DX(){
		if(sx && (timewait != 0)){
			anim.SetInteger("Stato", 4);
			anim.SetBool("Gira", true);

			sx = false;
		}
		Directon = true;
		anim.SetInteger("Stato", 1);
		rigidbody2D.AddForce(new Vector2(Acceleration, rigidbody2D.velocity.magnitude));
		if (We.Input.Attack == true)
			rigidbody2D.AddForce(new Vector2(Acceleration, rigidbody2D.velocity.magnitude));
	    timewait = 0.25f;
		print ("ok");
		dx = true;
	}

	void SX(){
		if (dx && (timewait != 0)){
			anim.SetInteger("Stato", 4);
			anim.SetBool("Gira", true);

			dx = false;		
		}
		Directon = false;
		anim.SetInteger("Stato", 1);
		rigidbody2D.AddForce(new Vector2(-Acceleration,0));
		if (We.Input.Attack == true)
			rigidbody2D.AddForce(new Vector2(-Acceleration,0));
		timewait = 0.25f;
		print ("ok");
		sx = true;
	}

	void DWN()
	{

		//float cazzetto = box.size.y;
		//cazzetto = 20f;
		//rigidbody2D.collider2D.
		anim.SetInteger("Stato", 2);
		//anim.SetBool ("abbass", false);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		isOnGround = true;
		if (isOnGround == false)
		{
			Vector3 otherPos = other.transform.position;
			Vector3 thisPos = this.transform.position;
			if (otherPos.y < thisPos.y)
			{
				isOnGround = true;
			}
		}
	}
	void OnCollisionExit2D(Collision2D other)
	{
		if (isOnGround == false)
		{
			isOnGround = true;
		}
	}
}