using UnityEngine;
using System.Collections;

public class StateManagement : MonoBehaviour
{
    public enum State
    {
        Unchanged,
        Stand,
        Walk,
        Jump,
        Attack,
        Crouch,
    }

    public State state = State.Stand;
    PhysicsManagement physics;

    void Awake()
    {
        physics = GetComponent<PhysicsManagement>();
    }

    void FixedUpdate()
    {
        state = GetState(physics);
    }

    public State GetState(PhysicsManagement physics)
    {
        if (We.Input.Jump == true)
        {
            return State.Jump;
        }
        if (We.Input.MoveLeft == true)
        {
            physics.Direction = false;
            return State.Walk;
        }
        if (We.Input.MoveRight == true)
        {
            physics.Direction = true;
            return State.Walk;
        }
        if (We.Input.MoveDown == true)
        {
            return State.Crouch;
        }
        if (We.Input.Attack2 == true)
        {
            return State.Attack;
        }
        return State.Stand;
    }
}
