using UnityEngine;
using System.Collections;

public class PhysicsManager : MonoBehaviour
{
    public static readonly int GROUND_LAYER = 8;
    public static readonly int PLAYER_LAYER = 13;
    public static readonly int MOVINGPLATFORM_LAYER = 14;
    public static readonly int PLAYERHIDE_LAYER = 15;
    public static readonly int HIDEOUT_LAYER = 16;
    public static readonly int ENEMY_LAYER = 11;
    public static readonly int ENEMYWEAPON_LAYER = 12;

    public static readonly int GROUND_MASK = 1 << 8 | 1 << 10 | 1 << 17;
    public static readonly int PLAYER_MASK = 1 << PLAYER_LAYER;
    public static readonly int MOVINGPLATFORM_MASK = 1 << MOVINGPLATFORM_LAYER;
    public static readonly int PLAYERHIDE_MASK = 1 << PLAYERHIDE_LAYER;
    public static readonly int HIDEOUT_MASK = 1 << HIDEOUT_LAYER;
    public static readonly int ENEMYALL_MASK = (1 << ENEMYWEAPON_LAYER) | (1 << ENEMY_LAYER);
	public static readonly int STAIR_MASK = 1 << 10;

    /// <summary>
    /// Rappresenta l'HUD collegata al personaggio.
    /// Non per forza deve essere un oggetto che rappresenta qualcosa di
    /// grafico, ma basti che abbia attaccato lo script HudHandler in modo
    /// da leggere e scrivere i valori necessari.
    /// </summary>
    public GameObject hud;

    public bool IsPlayableCharacter = false;
    public bool GravityDisabled = false;

    public float Health
    {
        get
        {
            if (hud != null)
                return hud.GetComponent<HudHandler>().ValueHealth;
            else
                return 1;
        }
        set
        {
            if (hud != null)
                hud.GetComponent<HudHandler>().ValueHealth = value;
        }
    }
    public float Stamina
    {
        get
        {
            if (hud != null)
                return hud.GetComponent<HudHandler>().ValueStamina;
            else
                return 1;
        }
        set
        {
            if (hud != null)
                hud.GetComponent<HudHandler>().ValueStamina = value;
        }
    }

    /// <summary>
    /// Dice se il personaggio sta collidendo sul terreno o meno
    /// </summary>
    public bool IsOnGround;
    /// <summary>
    /// Dice se il personaggio sta in salto o meno
    /// </summary>
    public bool Jumping = false;

    public float Gravity = 8.0f;
    /// <summary>
    /// Numero di pixel percorribili al secondo in camminata
    /// </summary>
    public float WalkSpeed = 32.0f;
    /// <summary>
    /// Numero di pixel percorribili al secondo in corsa
    /// </summary>
    public float RunSpeed = 160.0f;
    /// <summary>
    /// Forza del salto (praticamente la velocità con cui salta).
    /// Dato che il salto è pesato, questa sarà la massima forza
    /// che un salto può avere.
    /// </summary>
    public float JumpStrengthMaximum = 256.0f;
    /// <summary>
    /// Il minimo di forza applicabile nel salto.
    /// </summary>
    public float JumpStrengthMinimum = 64.0f;
    /// <summary>
    /// Forza della scivolata. Con questa si decide la sua velocità.
    /// </summary>
    public float ScivolataForza = 250.0f;
    /// <summary>
    /// Inerzia della scivolata. Determina la forza che oppone quella della
    /// scivolata, in modo da farlo rallentare più o meno velocemente.
    /// </summary>
    public float ScivolataInerzia = 5.0f;

    /// <summary>
    /// Quanta stamina viene recuperata mentre si è fermi
    /// </summary>
    public int RecuperoStamina = 10;
    /// <summary>
    /// Quanta stamina viene consumata con la corsa
    /// Il valore è diluito in un secondo
    /// </summary>
    public int ConsumoStaminaCorsa = 35;
    /// <summary>
    /// Quanta stamina viene consumata con il salto
    /// Il valore diminuisce la stamina di netto
    /// </summary>
    public int ConsumoStaminaSalto = 40;
	/// <summary>
	/// Quanta stamina viene consumata con il l'attacco
	/// Il valore diminuisce la stamina di netto
	/// </summary>
	public int ConsumoStaminaAttacco = 45;
	/// <summary>
	/// Quanta stamina viene consumata con il la difesa
	/// Il valore diminuisce la stamina di netto
	/// </summary>
	public int ConsumoStaminaDifesa = 30;
	/// <summary>
	/// Quanta vita viene consumata dal contatto con le frecce
	/// </summary>
	public int ConsumoVitaFrecce = 15;
	/// <summary>
	/// Quanta vita viene consumata dal contatto con le trappole
	/// </summary>
	public int ConsumoVitaTrappole = 40;
	/// <summary>
	/// Quanta vita viene consumata dal contatto con l'arma del nemico
	/// </summary>
	public int ConsumoVitaColpoArmaNemico = 20;
	/// <summary>
	/// Quanta vita viene consumata dal contatto con il nemico
	/// </summary>
	public int ConsumoVitaColpoNemico = 5;
    /// <summary>
    /// La velocità orizzontale e verticale del personaggio
    /// Ogni valore è espresso per pixel al secondo.
    /// </summary>
    public int ConsumoVitaColpoArmaBoss = 30;
	/// <summary>
	/// Quanta vita viene consumata dal contatto con il nemico
	/// </summary>
	public int ConsumoVitaColpoBoss = 15;

    public AudioClip[] SFX;

    /// <summary>
    /// La velocità orizzontale e verticale del personaggio
    /// Ogni valore è espresso per pixel al secondo.
    /// </summary>
	/// 
    public Vector2 speed;

	private bool colpito = false;

    public bool Hide
    {
        get { return isHide; }
        set
		{
            rigidbody2D.isKinematic = isHide = value;
            gameObject.layer = value ? PLAYERHIDE_LAYER : PLAYER_LAYER;
        }
    }


    private bool isHide;

    /// <summary>
    /// Ottiene oppure imposta lo stato corrente
    /// </summary>
    public StateManager.State State
    {
        get
        {
            AnimationManager anim = GetComponent<AnimationManager>();
            if (anim != null)
                return anim.state;
            else return StateManager.State.Unpressed;
        }
        set
        {
            AnimationManager anim = GetComponent<AnimationManager>();
            if (anim != null)
                anim.state = value;
        }
    }
    private StateManager.State PrevState;

    /// <summary>
    /// Ottiene oppure imposta la direzione corrente del personaggio
    /// </summary>
    public bool Direction
    {
        get { return transform.transform.rotation.y == 0; }
        set
        {
            Quaternion rot = transform.transform.rotation;
            rot.y = value ? 0 : 180;
            transform.transform.rotation = rot;
        }
    }

    #region PARTE DEDICATA AI RAYCAST
    /// <summary>
    /// Processa un raycast che lavora orizzontalmente
    /// </summary>
    /// <param name="y">Posizione Y dal quale far partire il raggio</param>
    /// <param name="width">Lunghezza del raggio</param>
    /// <param name="mask">Maschera su cui lavorare</param>
    /// <param name="color">Colore del raggio in modalità debug</param>
    /// <returns>true se collide, altrimenti false</returns>
    public static bool EvaluateRaycastH(Transform t, float x, float y, float width, int mask, Color color)
    {
        float distance = Mathf.Abs(width);
        Vector2 vOrigin = new Vector2(t.position.x + x, t.position.y + y);
        Vector3 vDirection = width >= 0 ? Vector3.right : Vector3.left;

        Debug.DrawRay(vOrigin, vDirection * distance, color);
        if (Physics2D.Raycast(vOrigin, vDirection, distance, mask))
            return true;
        return false;
    }

    /// <summary>
    /// Processa un raycast che lavora orizzontalmente
    /// </summary>
    /// <param name="y">Posizione Y dal quale far partire il raggio</param>
    /// <param name="width">Lunghezza del raggio</param>
    /// <param name="mask">Maschera su cui lavorare</param>
    /// <param name="color">Colore del raggio in modalità debug</param>
    /// <returns>true se collide, altrimenti false</returns>
    public bool EvaluateRaycastH(float x, float y, float width, int mask, Color color)
    {
        return EvaluateRaycastH(transform, x, y, Direction ? width : -width, mask, color);
    }

    /// <summary>
    /// Processa un raycast che lavora verticalmente
    /// </summary>
    /// <param name="x">Posizione X dal quale far partire il raggio</param>
    /// <param name="height">Altezza del raggio</param>
    /// <param name="mask">Maschera su cui lavorare</param>
    /// <param name="color">Colore del raggio in modalità debug</param>
    /// <returns>true se collide, altrimenti false</returns>
    public bool EvaluateRaycastV(float x, float y, float width, int mask, Color color)
    {
        float distance = width;
        Vector2 vOrigin = new Vector2(transform.position.x + x, transform.position.y + y);
        Vector3 vDirection = Vector3.down;

        Debug.DrawRay(vOrigin, vDirection * distance, color);
        if (Physics2D.Raycast(vOrigin, vDirection, distance, mask))
            return true;
        return false;
    }

    /// <summary>
    /// Controlla se sotto il personaggio c'è il pavimento o meno
    /// </summary>
    /// <returns></returns>
    public bool CheckGround()
    {
        return EvaluateRaycastH(Direction ? -0.14f : +0.14f, -0.45f, 0.28f, GROUND_MASK, Color.green);
    }

    /// <summary>
    /// Controlla se sotto il personaggio c'è il pavimento o meno
    /// Controllo alternativo, con due raycast verticali ai lati del personaggio
    /// </summary>
    /// <returns></returns>
    public bool CheckMovingPlatform()
    {
        bool b1 = EvaluateRaycastV(-0.14f, -0.28f, 0.22f, MOVINGPLATFORM_MASK, Color.cyan);
        bool b2 = EvaluateRaycastV(+0.14f, -0.28f, 0.22f, MOVINGPLATFORM_MASK, Color.cyan);
        return b1 | b2;
        //return EvaluateRaycastH(Direction ? -0.14f : +0.14f, -0.48f, 0.28f, groundMask, Color.green);
    }

	public bool CheckStairs()
	{
		bool b1 = EvaluateRaycastV(-0.14f, -0.28f, 0.22f, STAIR_MASK, Color.cyan);
		bool b2 = EvaluateRaycastV(+0.14f, -0.28f, 0.22f, STAIR_MASK, Color.cyan);
		return b1 | b2;
		//return EvaluateRaycastH(Direction ? -0.14f : +0.14f, -0.48f, 0.28f, groundMask, Color.green);
	}

	/// <summary>
	/// Controlla se sopra il personaggio c'è una piataforma o meno
	/// </summary>
	/// <returns></returns>
	public bool CheckMovingPlatform2(){	
		if(State != StateManager.State.Scivolata && State != StateManager.State.Crouch)
			return EvaluateRaycastH(-0.04f, 0.45f, 0.11f, MOVINGPLATFORM_MASK, Color.red);
		else
			return EvaluateRaycastH(-0.04f, 0.28f, 0.11f, MOVINGPLATFORM_MASK, Color.yellow);
		//return EvaluateRaycastH(Direction ? -0.14f : +0.14f, -0.48f, 0.28f, groundMask, Color.green);
	}
	/// <summary>
	/// Controlla se sopra il personaggio c'è il soffitto o meno
	/// </summary>
	/// <returns></returns>
	public bool CheckCeiling(){	
		if(State != StateManager.State.Scivolata && State != StateManager.State.Crouch)
		    return EvaluateRaycastH(-0.04f, 0.45f, 0.11f, GROUND_MASK, Color.red);
		else
			return EvaluateRaycastH(-0.04f, 0.28f, 0.11f, GROUND_MASK, Color.yellow);
		//return EvaluateRaycastH(Direction ? -0.14f : +0.14f, -0.48f, 0.28f, groundMask, Color.green);
	}
    /// <summary>
    /// Controlla se la visuale del nemico incontra un suo nemico ad una certa
    /// distanza né troppo vicina né troppo lontana. Usata durante le fasi di guardia.
    /// </summary>
    /// <returns></returns>
    public bool CheckEnemyNear()
    {
        return EvaluateRaycastH(0.0f, 0.48f, 1.0f, PLAYER_MASK, Color.magenta) ||
            EvaluateRaycastH(0.0f, -0.24f, 1.0f, PLAYER_MASK, Color.magenta);
    }

    /// <summary>
    /// Controlla se la visuale del nemico incontra un suo nemico a distanza
    /// ravvicinata. Usata durante le fasi di guardia.
    /// </summary>
    /// <returns></returns>
    public bool CheckEnemyAround()
    {
        return EvaluateRaycastH(0.0f, 0.48f, 1.5f, PLAYER_MASK, Color.blue) ||
            EvaluateRaycastH(0.0f, 0.0f, 1.5f, PLAYER_MASK, Color.blue);
    }

    /// <summary>
    /// Controlla se il nemico è dietro di lui
    /// </summary>
    /// <returns></returns>
    public bool CheckEnemyBack()
    {
        return EvaluateRaycastH(0.0f, 0.48f, -0.5f, PLAYER_MASK, Color.blue) ||
            EvaluateRaycastH(0.0f, 0.0f, -0.5f, PLAYER_MASK, Color.blue);
    }

    /// <summary>
    /// Controlla se la visuale del nemico incontra un suo nemico a distanza
    /// ravvicinata. Usata durante le fasi di guardia.
    /// </summary>
    /// <returns></returns>
    public bool CheckHideout()
    {
        return EvaluateRaycastH(Direction ? -0.14f : +0.14f, -0.48f, 0.28f, HIDEOUT_MASK, Color.yellow);
    }

    public bool CheckEnemyHitted()
    {
        bool b1 = EvaluateRaycastV(-0.17f, 0.42f, 0.96f, ENEMYALL_MASK, Color.yellow);
        bool b2 = EvaluateRaycastV(+0.17f, 0.42f, 0.96f, ENEMYALL_MASK, Color.yellow);
        return b1 | b2;
    }

    /// <summary>
    /// Controlla se si è vicini al bordo del pavimento, usato solitamente per
    /// evitare di cadere dalla piattaforma corrente.
    /// </summary>
    /// <returns></returns>
    public bool CheckNearEdge()
    {
        float distanza = 0.24f;
        return EvaluateRaycastV(Direction ? -distanza : +distanza, 0.0f, 0.72f, GROUND_MASK, Color.green) == false;
    }

    /// <summary>
    /// Controlla se si è vicini ad un muro. Usato solitamente per evitare che
    /// l'entità si avvicini troppo al muro inutilmente e che si fermi già da
    /// molto prima.
    /// </summary>
    /// <returns></returns>
    public bool CheckNearWall()
    {
        return EvaluateRaycastH(0.0f, -0.24f, 1.0f, GROUND_MASK, Color.green) == true;
    }
    #endregion

    void Awake()
    {

    }

    void FixedUpdate()
    {
        if (IsPlayableCharacter)
        {
            if (CheckEnemyHitted())
            {
                GetComponent<Animator>().SetBool("Colpito", true);
            }
        }
        if (CheckGround() || CheckMovingPlatform())
        {
            if (colpito == true && speed.y < 0)
            {
                colpito = false;
                IsOnGround = true;
                State = StateManager.State.Stand;
                GetComponent<Animator>().SetBool("Colpito", false);
            }
            else if (colpito == false)
            {
                IsOnGround = true;
                Jumping = false;
                if (!CheckMovingPlatform())
                    speed.y = 0;
                else if (State != StateManager.State.Jumping)
                    speed.y = -40;
                if (State == StateManager.State.Unpressed ||
                    PrevState == StateManager.State.Falling)
                    State = StateManager.State.Stand;
            }
        }
        else if (!GravityDisabled)
        {
            IsOnGround = false;
            if (speed.y < Gravity)
                State = StateManager.State.Falling;
        }

        if (colpito == false)
        {
            Stamina += RecuperoStamina * Time.deltaTime;

            switch (State)
            {
                case StateManager.State.Stand:
                    if (IsOnGround == true)
                    {
                        speed.x = 0.0f;
                    }
                    else
                    {
                        if (speed.y >= JumpStrengthMinimum)
                            speed.y = JumpStrengthMinimum;
                    }
                    break;
                case StateManager.State.Walk:
                    speed.x = Direction ? +WalkSpeed : -WalkSpeed;
                    break;
                case StateManager.State.Run:
                    speed.x = Direction ? +RunSpeed : -RunSpeed;
                    Stamina -= ConsumoStaminaCorsa * Time.deltaTime;
                    if (Stamina <= 0)
                        State = StateManager.State.Walk;
                    break;
                case StateManager.State.Jumping:
                    if (IsOnGround == true && Stamina >= ConsumoStaminaSalto)
                    {
                        IsOnGround = false;
                        Jumping = true;
                        speed.y = JumpStrengthMaximum;
                        Stamina -= ConsumoStaminaSalto;
                    }

                    break;
                case StateManager.State.Falling:
                    if (IsOnGround)
                        State = StateManager.State.Stand;
                    break;
                case StateManager.State.PreScivolata:
                    speed.x = Direction ? +ScivolataForza : -ScivolataForza;
                    State = StateManager.State.Scivolata;
                    break;
                case StateManager.State.Scivolata:
                    speed.x += (Direction ? -ScivolataInerzia : +ScivolataInerzia);
                    if (Direction ? speed.x <= 0 : speed.x >= 0)
                    {
                        speed.x = 0;
                        State = StateManager.State.Crouch;
                    }
                    break;

                case StateManager.State.Attack:
                    //Attacco logica
                    if (Stamina > ConsumoStaminaAttacco)
                        Stamina -= ConsumoStaminaAttacco;
                    break;

                case StateManager.State.Attack2:
                    //Attacco2 logica
                    if (Stamina > ConsumoStaminaAttacco)
                        Stamina -= ConsumoStaminaAttacco;
                    break;

                case StateManager.State.Defense:
                    //Difesa logica
                    if (Stamina >= 0)
                    {
                        Stamina -= ConsumoStaminaDifesa * Time.deltaTime;

                    }
                    if (Stamina <= ConsumoStaminaDifesa)
                        State = StateManager.State.Stand;
                    break;
                case StateManager.State.Died:
                    break;
            }
        }
        if (!IsOnGround && !GravityDisabled)
            speed.y -= Gravity;
        PrevState = State;
        rigidbody2D.velocity = new Vector3(speed.x * Time.deltaTime, speed.y * Time.deltaTime, 0.0f);

        if (CheckCeiling() && IsOnGround)
        {
            Health = 0;
        }
        if (CheckMovingPlatform2() && IsOnGround && !CheckMovingPlatform() && this.transform.position.y > 6.5f)
        {
            Health = 0;
        }
    }


    void OnCollisionEnter2D(Collision2D obj)
    {
        if (!colpito)
        {
            bool ahia = false;
            if (obj.gameObject.tag == "Arrow")
            {
                if (State != StateManager.State.Defense)
                {
                    audio.Play();
                    GetComponent<Animator>().SetBool("Colpito", true);
                    Health -= ConsumoVitaFrecce;
                    ahia = true;

                }
                Destroy(obj.gameObject);
            }
            else if (obj.gameObject.tag == "TRAP")
            {
                GetComponent<Animator>().SetBool("Colpito", true);
                Health -= ConsumoVitaTrappole;
                if (SFX.Length >= 5)
                {
                    audio.clip = SFX[4];
                    audio.Play();
                }
                audio.Play();
                ahia = true;
            }
            else if (obj.gameObject.tag == "Enemy")
            {
                if (State != StateManager.State.Defense)
                {
                    if (SFX.Length >= 5)
                    {
                        audio.clip = SFX[1];
                        audio.Play();
                    }
                    Health -= ConsumoVitaColpoNemico;
                    ahia = true;
                }
            }
            else if (obj.gameObject.tag == "EnemyWeapon")
            {
                if (State != StateManager.State.Defense)
                {
                    if (SFX.Length >= 5)
                    {
                        audio.clip = SFX[2];
                        audio.Play();
                    }
                    Health -= ConsumoVitaColpoArmaNemico;
                    ahia = true;
                }
            }
			else if (obj.gameObject.tag == "Boss")
			{
				if (State != StateManager.State.Defense)
                {
                    if (SFX.Length >= 5)
                    {
                        audio.clip = SFX[1];
                        audio.Play();
                    }
					Health -= ConsumoVitaColpoBoss;
					ahia = true;
				}
			}
            else if (obj.gameObject.tag == "BossWeapon")
            {
                if (State != StateManager.State.Defense)
                {
                    if (SFX.Length >= 5)
                    {
                        audio.clip = SFX[2];
                        audio.Play();
                    }
                    Health -= ConsumoVitaColpoArmaBoss;
                    ahia = true;
                }
            }
            else if (obj.gameObject.tag == "PlayerWeapon" && !IsPlayableCharacter)
            {
                audio.Play();
                Health -= 5;
            }
            // Controlla se è stato colpito o meno
            if (ahia == true)
            {
                Direction = obj.transform.position.x > transform.position.x;
                speed.x = Direction ? -JumpStrengthMinimum : +JumpStrengthMinimum;
                speed.y = JumpStrengthMaximum;
                IsOnGround = false;
                colpito = true;
                GetComponent<Animator>().SetBool("Colpito", true);
            }
        }
    }
}
