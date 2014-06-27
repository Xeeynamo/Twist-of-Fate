using UnityEngine;
using System.Collections;

public class PhysicsManagement : MonoBehaviour
{
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
    public float RunSpeed = 32.0f;
    public float JumpStrength = 96.0f;
    public float JumpMinimum = 32.0f;
    public float JumpMaximum = 4.0f;

    public float speedX = 0;
    public float speedY = 0;

    private int groundMask = 1 << 8 | 1 << 10;
    private int stairMask = 1 << 10 | 1 << 11;
    private float raycastWidth = 0.45f;
    StateManagement state;

    void Awake()
    {
        state = GetComponent<StateManagement>();
    }

    void FixedUpdate()
    {
        if (Physics2D.Raycast(new Vector2(transform.position.x - 0.05f, transform.position.y), Vector3.down, raycastWidth, groundMask) ||
            Physics2D.Raycast(new Vector2(transform.position.x + 0.05f, transform.position.y), Vector3.down, raycastWidth, groundMask))
        {
            IsOnGround = true;
            Jumping = false;
            speedY = 0;
        }
        else
        {
            IsOnGround = false;
        }

        switch (state.state)
        {
            case StateManagement.State.Stand:
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
            case StateManagement.State.Walk:
                speedX = Direction ? +WalkSpeed : -WalkSpeed;
                break;
            case StateManagement.State.Jump:
                if (IsOnGround == true)
                {
                    IsOnGround = false;
                    Jumping = true;
                    speedY = JumpStrength;
                }
                break;
        }

        speedY -= Gravity;

        rigidbody2D.velocity = new Vector3(speedX * Time.deltaTime, speedY * Time.deltaTime, 0.0f);
    }
}
