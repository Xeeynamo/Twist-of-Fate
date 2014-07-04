using UnityEngine;
using System.Collections;

public class Richard : MonoBehaviour
{
	public static Transform _Transform;

    PhysicsManager physManager;

	StateManager.State PrevState
	{
		get { return physManager.State; }
	}

	float TimerScatto = 0.5f;
	public float TimerScattoRimastoLeft = 0f;
	public float TimerScattoRimastoRight = 0f;
	public bool TastoDirezionalePrecedentementePremuto = false;
	public bool ScattoAttivato = false;

    // Use this for initialization
    void Start()
    {
        physManager = GetComponent<PhysicsManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
	{
		StateManager.State state = PrevState;
		switch (state)
		{
		case StateManager.State.Scivolata:
			break;
		default:
			state = getState ();
			if (PrevState == StateManager.State.Crouch && state == StateManager.State.Walk)
			{
				state = StateManager.State.PreScivolata;
			}
			break;
		}
		physManager.State = state;
		_Transform = transform;
    }

	StateManager.State getState()
	{
		TimerScattoRimastoLeft += Time.deltaTime;
		TimerScattoRimastoRight += Time.deltaTime;
		if (We.Input.Jump == true)
		{
			return StateManager.State.Jump;
		}
		if (We.Input.MoveLeft == true)
		{
			physManager.Direction = false;
			StateManager.State state;
			if(ScattoAttivato == true || (TastoDirezionalePrecedentementePremuto == false && TimerScattoRimastoLeft < TimerScatto))
			{
				ScattoAttivato = true;
				state = StateManager.State.Run;
			}
			else
				state = StateManager.State.Walk;
			if (TastoDirezionalePrecedentementePremuto == false)
			{
				TastoDirezionalePrecedentementePremuto = true;
				TimerScattoRimastoLeft = 0;
			}
			return state;
		}
		if (We.Input.MoveRight == true)
		{
			physManager.Direction = true;
			StateManager.State state;
			if(ScattoAttivato == true || (TastoDirezionalePrecedentementePremuto == false && TimerScattoRimastoRight < TimerScatto))
			{
				ScattoAttivato = true;
				state = StateManager.State.Run;
			}
			else
				state = StateManager.State.Walk;
			if (TastoDirezionalePrecedentementePremuto == false)
			{
				TastoDirezionalePrecedentementePremuto = true;
				TimerScattoRimastoRight = 0;
			}
			return state;
		}
		TastoDirezionalePrecedentementePremuto = false;
		ScattoAttivato = false;
		if (We.Input.MoveDown == true)
		{
			transform.position = new Vector2(transform.position.x, transform.position.y-0.15f);
			return StateManager.State.Crouch;
		}
		if (We.Input.Attack2 == true)
		{
			return StateManager.State.Attack;
		}
		return StateManager.State.Stand;
	}
}
