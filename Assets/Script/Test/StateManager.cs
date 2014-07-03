using UnityEngine;
using System.Collections;

public class StateManager
{
    public enum State
    {
        Unchanged,
        Stand,
        Walk,
        Jump,
        Attack,
        Crouch,
        /// <summary>
        /// Quando si gira
        /// </summary>
        Turn,
        /// <summary>
        /// Entità allerta
        /// </summary>
        Alert,
    }

    public static State getStateFromInput(PhysicsManager physics)
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
