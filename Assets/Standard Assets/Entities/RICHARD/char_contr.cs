using UnityEngine;
using System.Collections;

public class char_contr : MonoBehaviour
{
	float jump = 0.0f;
	private Animator anim;
	private Transform play;
	
	/// <summary>
	/// Accelerazione del movimento orizzontale del personaggio
	/// </summary>
	public float acceleration;

	/// <value>L'accelerazione da impostare</value>
	public float Acceleration
	{
		get { return acceleration; }
		set { acceleration = value; }
	}

	/// <summary>
	/// true = guarda in avanti
	/// false = si gira
	/// </summary>
	/// <value><c>true</c> if directon; otherwise, <c>false</c>.</value>
	public bool Directon 
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
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		float comy = 0.0f;

		if (We.Input.MoveRight == true)
		{
			Directon = true;
			anim.SetInteger("Stato", 1);
			rigidbody2D.AddForce(new Vector2(Acceleration,0));

		}
		else if (We.Input.MoveLeft == true)
		{
			Directon = false;
			anim.SetInteger("Stato", 1);
			rigidbody2D.AddForce(new Vector2(-Acceleration,0));
		}

		else if (We.Input.Jump)
		{
			rigidbody2D.AddForce(new Vector2(0,600));
		}
		else
		{
			anim.SetInteger("Stato", 0);
		}

		if (comy < 0)
		{
			anim.SetInteger("Stato", 2);
		}
	}
}
 