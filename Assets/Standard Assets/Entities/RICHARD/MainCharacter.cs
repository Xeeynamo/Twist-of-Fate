using UnityEngine;
using System.Collections;

public class MainCharacter : MonoBehaviour
{
	public enum State
	{
		Stop,
		Walk,
		WalkStopping,
		GoingDown,
		GoingUp,
		Jump,
	}
	
	// === Parte dedicata all'animazione
	private SpriteRenderer spriteRenderer;
	private We.Animation anim;
	public Sprite[] AnimStop;
	public Sprite[] AnimWalk;
	public Sprite[] AnimWalkStopping;
	public Sprite[] AnimGoingDown;
	public Sprite[] AnimGoingUp;
	public Sprite[] AnimJump;
	public bool isAnimFinished;
	
	// === Parte dedicata alla fisica
	private We.Physics physics;
	// velocità del personaggio
	const float SPEED = 1f;
	// gravità applicata al personaggio
	const float GRAVITY = 0.003f;
	// massima forza di gravità
	const float MAXGRAVITY = 0.07f;
	// forza di salto del personaggio
	const float JUMP = 0.07f;
	// input del personaggio
	private Vector3 inputMovement;
	// se il tasto del salto è stato premuto
	private bool inputJump;
	// se precedentemente il tasto del salto è stato premuto
	private bool inputPrevJump;
	// evita di far saltare il personaggio; può essere utile ;)
	private bool lockJump;
	// determina se il personaggio è in aria o meno
	private bool isInAir;
	// velocità Y
	private float speedY;
	
	public State CurrentState
	{
		get { return (State)anim.CurrentAnimation; }
		set { anim.CurrentAnimation = (int)value; }
	}
	
	// chiamato solo nella fase di inizializzazione
	void Start ()
	{
		// inizializza le animazioni
		spriteRenderer = renderer as SpriteRenderer;
		CreateAnimations();
		CurrentState = State.Stop;
		
		// inizializza la fisica
		physics = new We.Physics ();
		
		isInAir = true;
		lockJump = false;
		speedY = 0.0f;
	}
	
	void ProcessGround()
	{
		MoveGround ();
		
		if (inputJump == true && lockJump == false)
		{
			// se è stato premuto il pulsante di salto
			isInAir = true;
			CurrentState = State.Jump;
			speedY = JUMP;
		}
		// controlla le azioni sui soli tasti direzionali
		else if (inputMovement.x != 0)
		{
			// viene premuto destra o sinistra
			CurrentState = State.Walk;
		}
		else if (inputMovement.y < 0)
		{
			// pulsante giù
			CurrentState = State.GoingDown;
		}
		else
		{
			// non viene premuto niente
			switch (CurrentState)
			{
				// se prima di fermarsi stava camminando
			case State.Walk:
				// animazione che si sta per fermare
				CurrentState = State.WalkStopping;
				break;
			case State.WalkStopping:
				break;
			case State.GoingDown:
				// il personaggio era abbassato, ora lo alza con l'animazione
				CurrentState = State.GoingUp;
				break;
			case State.GoingUp:
				break;
			default:
				// gestione di tutti gli altri casi
				//CurrentState = State.Stop;
				break;
			}
		}
		
		spriteRenderer.sprite = anim.CurrentFrame;
		
		if (anim.Process () == false)
		{
			// intercettata la fine di un'animazione
			switch (CurrentState)
			{
			case State.WalkStopping:
				CurrentState = State.Stop;
				break;
			case State.GoingUp:
				CurrentState = State.Stop;
				break;
			default:
				CurrentState = State.Stop;
				break;
			}
		}
	}
	void ProcessAir()
	{
		MoveAir ();
	}
	
	// viene chiamato ad ogni frame
	void Update ()
	{
		/*Vector3 pos = transform.position;
		Quaternion rot = transform.rotation;
		
		Vector3 direction = physics.Process ();
		ProcessAnimations (direction);
		pos.x += direction.x;
		pos.y += direction.y;
		pos.z += direction.z;
		if (direction.x > 0)
			rot.y = 0;
		else if (direction.x < 0)
			rot.y = 180;
		
		transform.position = pos;
		transform.rotation = rot;*/

		ProcessInput ();
		
		if (isInAir == true)
		{
			lockJump = true;
			ProcessAir ();
		}
		else
		{
			// evita che il personaggio salti se tieni premuto il pulsante salto
			if (lockJump == true && inputPrevJump == false)
				lockJump = false;
			ProcessGround ();
		}
	}
	
	// crea le animazioni per l'entità
	void CreateAnimations()
	{
		anim = new We.Animation ();
		anim.AddAnimation (AnimStop, 0, 3);
		anim.AddAnimation (AnimWalk, 4, 3);
		anim.AddAnimation (AnimWalkStopping, -1, 3);
		anim.AddAnimation (AnimGoingDown, 14, 2);
		anim.AddAnimation (AnimGoingUp, -1, 2);
		anim.AddAnimation (AnimJump, 0, 3);
	}

	void ProcessAnimations(Vector3 direction)
	{
		if (physics.IsInAir == true)
		{
			// tutto ciò che accade quando l'entità è in aria
			if (direction.y > 0)
			{
				// se l'entità sta andando verso l'alto, allora è perché sta saltando
				CurrentState = State.Jump;
			}
			else if (direction.y < 0)
			{
				// l'entità sta precipitando
			}
			else
			{
				// l'entità è sospesa in aria
			}
			CurrentState = State.Walk;
		}
		else
		{
			// tutto ciò che accade quando l'entità è a terra
			if (direction.x != 0)
			{
				// tutto ciò che accade quando l'entità è in movimento
				if (We.Input.MoveLeft == true ||
				    We.Input.MoveRight == true)
				{
					// l'entità è in movimento
					CurrentState = State.Walk;
				}
				else
				{
					// l'entità si sta per fermare
					CurrentState = State.WalkStopping;
				}
			}
			else
			{
				// tutto ciò che accade quando l'entità è ferma
				switch (CurrentState)
				{
				case State.Walk:
					// se l'entità stava precedentemente camminando
					// ed ora né LEFT né RIGHT sono premuti, allora
					// fa partire l'animazione che si sta per fermare.
					CurrentState = State.WalkStopping;
					break;
				case State.WalkStopping:
					// se l'animazione che si sta per fermare è finita,
					// allora imposta l'animazione del personaggio fermo.
					CurrentState = State.Stop;
					break;
				case State.GoingDown:
					if (We.Input.MoveDown == false)
					{
						// l'entità era precedentemente acovacciata e,
						// visto che il tasto giù non è più tenuto premuto,
						// allora esegue l'animazione che si rialza
						CurrentState = State.GoingUp;
					}
					break;
				}
				if (We.Input.MoveDown == true)
				{
					// l'entità si sta per abbassare
					CurrentState = State.GoingDown;

				}
			}
		}
		isAnimFinished = anim.Process();
		spriteRenderer.sprite = anim.CurrentFrame;
	}
	
	// processa l'input
	void ProcessInput()
	{
		inputMovement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
		if (inputMovement.y < 0)
			inputMovement.y = -1;
		else if (inputMovement.y > 0)
			inputMovement.y = +1;
		inputPrevJump = inputJump;
		inputJump = Input.GetButton ("Jump");
	}
	
	void OnCollisionEnter2D(Collision2D other)
	{
		if (isInAir == true)
		{
			Vector3 otherPos = other.transform.position;
			Vector3 thisPos = this.transform.position;
			if (otherPos.y > thisPos.y)
			{
				// === collisione sopra il personaggio
				// la velocità Y viene resettata, ma il personaggio
				// è ancora in aria, dato che ha solo "sbattuto"
				// con la testa e deve tornare giù
				speedY = 0.0f;
			}
			else
			{
				// == collisione ai piedi del personaggio
				// resettiamo la velocità Y, gli diciamo che
				// non è più in aria e diamogli l'animazione
				// dell'atterraggio
				speedY = 0.0f;
				isInAir = false;
				CurrentState = State.Stop;
			}
		}
	}
	void OnCollisionStay2D(Collision2D other)
	{
	}
	void OnCollisionExit2D(Collision2D other)
	{
		if (isInAir == false)
		{
			isInAir = true;
		}
	}
	
	// muove il personaggio quando esso è a terra
	void MoveGround()
	{
		Vector3 pos = transform.position;
		Quaternion rot = transform.rotation;
		pos.x += inputMovement.x * SPEED * Time.smoothDeltaTime;
		if (inputMovement.x > 0)
			rot.y = 0;
		else if (inputMovement.x < 0)
			rot.y = 180;
		transform.position = pos;
		transform.rotation = rot;
	}
	// muove il personaggio quando esso è in aria
	void MoveAir()
	{
		Vector3 pos = transform.position;
		Quaternion rot = transform.rotation;
		pos.x += inputMovement.x * SPEED;
		pos.y += speedY;
		speedY -= GRAVITY;
		if (speedY < -MAXGRAVITY)
			speedY = -MAXGRAVITY;
		if (inputMovement.x > 0)
		{
			rot.y = 0;
		}
		else if (inputMovement.x < 0)
		{
			rot.y = 180;
		}
		transform.position = pos;
		transform.rotation = rot;
	}
}
