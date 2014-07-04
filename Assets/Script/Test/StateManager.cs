using UnityEngine;
using System.Collections;

public class StateManager
{
    public enum State
    {
        /// <summary>
        /// Usato per dire che, dall'input, non è stato premuto alcun pulsante.
        /// Ciò significa che non ci saranno veri e propri cambi di stato o
        /// nuove animazioni da eseguire.
        /// </summary>
        Unpressed,
        /// <summary>
        /// Personaggio nella posizione standard.
        /// </summary>
        Stand,
		Walk,
		Run,
        Jumping,
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

