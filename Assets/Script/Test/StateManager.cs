using UnityEngine;
using System.Collections;

public class StateManager
{
    public enum State
    {
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
        /// <summary>
        /// Quando il personaggio sta cadendo.
        /// Usato quando si salta ed il personaggio comincia a cadere, oppure
        /// quando il personaggio cade dal bordo di una piattaforma.
        /// </summary>
        Falling,
    }
}

