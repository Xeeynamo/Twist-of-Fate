using UnityEngine;
using System.Collections;

public class PhysicsManager : MonoBehaviour
{
    public static int groundMask = 1 << 8 | 1 << 10;
    public static int playerMask = 1 << 13;

    /// <summary>
    /// Rappresenta l'HUD collegata al personaggio.
    /// Non per forza deve essere un oggetto che rappresenta qualcosa di
    /// grafico, ma basti che abbia attaccato lo script HudHandler in modo
    /// da leggere e scrivere i valori necessari.
    /// </summary>
    public GameObject hud;

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
                return hud.GetComponent<HudHandler>().ValueMana;
            else
                return 1;
        }
        set
        {
            if (hud != null)
                hud.GetComponent<HudHandler>().ValueMana = value;
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
    /// La velocità orizzontale e verticale del personaggio
    /// Ogni valore è espresso per pixel al secondo.
    /// </summary>
    public Vector2 speed;

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
        float distance = width;
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
        return EvaluateRaycastH(Direction ? -0.14f : +0.14f, -0.48f, 0.28f, groundMask, Color.green);
    }


    /// <summary>
    /// Controlla se la visuale del nemico incontra un suo nemico ad una certa
    /// distanza né troppo vicina né troppo lontana. Usata durante le fasi di guardia.
    /// </summary>
    /// <returns></returns>
    public bool CheckEnemyNear()
    {
        return EvaluateRaycastH(0.0f, 0.48f, 1.0f, playerMask, Color.magenta) ||
            EvaluateRaycastH(0.0f, -0.24f, 1.0f, playerMask, Color.magenta);
    }

    /// <summary>
    /// Controlla se la visuale del nemico incontra un suo nemico a distanza
    /// ravvicinata. Usata durante le fasi di guardia.
    /// </summary>
    /// <returns></returns>
    public bool CheckEnemyAround()
    {
        return EvaluateRaycastH(0.0f, 0.48f, 1.5f, playerMask, Color.blue) ||
            EvaluateRaycastH(0.0f, 0.0f, 1.5f, playerMask, Color.blue);
    }

    /// <summary>
    /// Controlla se si è vicini al bordo del pavimento, usato solitamente per
    /// evitare di cadere dalla piattaforma corrente.
    /// </summary>
    /// <returns></returns>
    public bool CheckNearEdge()
    {
        float distanza = 0.24f;
        return EvaluateRaycastV(Direction ? -distanza : +distanza, 0.0f, 0.72f, groundMask, Color.green) == false;
    }

    /// <summary>
    /// Controlla se si è vicini ad un muro. Usato solitamente per evitare che
    /// l'entità si avvicini troppo al muro inutilmente e che si fermi già da
    /// molto prima.
    /// </summary>
    /// <returns></returns>
    public bool CheckNearWall()
    {
        return EvaluateRaycastH(0.0f, -0.24f, 1.0f, groundMask, Color.green) == true;
    }

    void Awake()
    {

    }

    void FixedUpdate()
    {
        if (CheckGround())
        {
            IsOnGround = true;
            Jumping = false;
            speed.y = 0;
            if (State == StateManager.State.Unpressed ||
                PrevState == StateManager.State.Falling)
                State = StateManager.State.Stand;
        }
        else
        {
            IsOnGround = false;
            if (speed.y <= 0)
                State = StateManager.State.Falling;
        }

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
        }

        Stamina += RecuperoStamina * Time.deltaTime;
        speed.y -= Gravity;
        PrevState = State;
        rigidbody2D.velocity = new Vector3(speed.x * Time.deltaTime, speed.y * Time.deltaTime, 0.0f);
    }
}
