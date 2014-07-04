using UnityEngine;
using System.Collections;

public class StateManager
{
    public enum State
    {
        Unchanged,
		Stand,
		Walk,
		Run,
        Jump,
        Attack,
		Attack2,
		PreScivolata,
		Scivolata,
		Defence,
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
}

