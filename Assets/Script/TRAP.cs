using UnityEngine;
using System.Collections;

public class TRAP : MonoBehaviour
{
    public enum State
    {
        Wait,
        Attack,
        Return,
    }

    /// <summary>
    /// Ogni quanto la trappola fa effetto
    /// </summary>
    public float TimerUp = 5.0f;

    /// <summary>
    /// Tempo la quale la trappola rimane ferma giù
    /// </summary>
    public float TimerDown = 1.0f;

    /// <summary>
    /// Gravità applicata ad essa
    /// </summary>
    public float Gravity = 8.0f;

    /// <summary>
    /// Velocità di ritorno della trappola
    /// </summary>
    public float ReturnSpeed = 2.0f;

    /// <summary>
    /// Stato dell'oggetto
    /// </summary>
    State state;

    float initialPosY;
    float timer;
    Vector2 speed;

    void Start()
    {
        initialPosY = transform.position.y;
        this.state = State.Wait;
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case State.Wait:
                timer += Time.deltaTime;
                if (timer >= TimerUp)
                {
                    state = State.Attack;
                }
                speed.y = 0;
                break;
            case State.Attack:
                speed.y -= Gravity;
                if (PhysicsManager.EvaluateRaycastH(transform, -0.16f, -1.30f, 0.32f, PhysicsManager.GROUND_MASK, Color.green))
                {
                    speed.y = -speed.y / 2;
                }
                else if (speed.y > 0 && speed.y < 1)
                {
                    speed.y = 0;
                    timer = 0;
                    state = State.Return;
                }
                break;
            case State.Return:
                timer += Time.deltaTime;
                if (timer >= TimerDown)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + ReturnSpeed * Time.deltaTime, transform.position.z);
                    if (transform.position.y >= initialPosY)
                    {
                        transform.position = new Vector3(transform.position.x, initialPosY, transform.position.z);
                        state = State.Wait;
                        timer = 0;
                    }
                }
                break;
        }
        rigidbody2D.velocity = new Vector3(speed.x * Time.deltaTime, speed.y * Time.deltaTime, 0.0f);
    }
}
