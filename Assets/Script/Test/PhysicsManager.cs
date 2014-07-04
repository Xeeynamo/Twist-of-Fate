using UnityEngine;
using System.Collections;

public class PhysicsManager : MonoBehaviour
{
    private int groundMask = 1 << 8 | 1 << 10;
    private int stairMask = 1 << 10 | 1 << 11;
    private int playerMask = 1 << 13;

    public bool IsOnGround;
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
    public float JumpStrength = 256.0f;
    public float JumpMinimum = 64.0f;
    public float JumpMaximum = 4.0f;
    public float ScivolataForza = 250.0f;
    public float ScivolataInerzia = 5.0f;

    public float speedX = 0;
    public float speedY = 0;

    private float raycastWidth = 0.45f;

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
    public bool EvaluateRaycastH(float x, float y, float width, int mask, Color color)
    {
        float distance = width;
        Vector2 vOrigin = new Vector2(transform.position.x + x, transform.position.y + y);
        Vector3 vDirection = Direction ? Vector3.right : Vector3.left;

        Debug.DrawRay(vOrigin, vDirection * distance, color);
        if (Physics2D.Raycast(vOrigin, vDirection, distance, mask))
            return true;
        return false;
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
            speedY = 0;
            if (State == StateManager.State.Unpressed ||
                PrevState == StateManager.State.Falling)
                State = StateManager.State.Stand;
        }
        else
        {
            IsOnGround = false;
            if (speedY <= 0)
                State = StateManager.State.Falling;
        }

        switch (State) 
        {
            case StateManager.State.Stand:
                if (IsOnGround == true)
                {
                    speedX = 0.0f;
                }
                else
                {
                    if (speedY >= JumpMinimum)
                        speedY = JumpMinimum;
                }
                break;
            case StateManager.State.Walk:
                speedX = Direction ? +WalkSpeed : -WalkSpeed;
                break;
            case StateManager.State.Run:
                speedX = Direction ? +RunSpeed : -RunSpeed;
                break;
            case StateManager.State.Jumping:
                if (IsOnGround == true)
                {
                    IsOnGround = false;
                    Jumping = true;
                    speedY = JumpStrength;
                }
                break;
            case StateManager.State.Falling:
                if (IsOnGround)
                    State = StateManager.State.Stand;
                break;
            case StateManager.State.PreScivolata:
                speedX = Direction ? +ScivolataForza : -ScivolataForza;
                State = StateManager.State.Scivolata;
                break;
            case StateManager.State.Scivolata:
                speedX += (Direction ? -ScivolataInerzia : +ScivolataInerzia);
                if (Direction ? speedX <= 0 : speedX >= 0)
                {
                    speedX = 0;
                    State = StateManager.State.Crouch;
                }
                break;
        }

        speedY -= Gravity;
        PrevState = State;
        rigidbody2D.velocity = new Vector3(speedX * Time.deltaTime, speedY * Time.deltaTime, 0.0f);
    }
}
