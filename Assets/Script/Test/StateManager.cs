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
        /// <summary>
        /// Quando il personaggio salta (da fermo)
        /// </summary>
        Jumping,
        Attack,
		Attack2,
		PreScivolata,
		Scivolata,

        /// <summary>
        /// Posizione di difesa
        /// </summary>
		Defense,
        /// <summary>
        /// QUando si abbassa
        /// </summary>
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
        /// <summary>
        /// Si sta preparando per sferrare l'attacco
        /// </summary>
        PrepareAttack,
        /// <summary>
        /// Entità allerta, ma in questo caso indietreggia
        /// </summary>
        AlertBack,
        /// <summary>
        /// Entità si nasconde
        /// </summary>
        Hide,
        /// <summary>
        /// Entità muore
        /// </summary>
        Died,
    }
}

