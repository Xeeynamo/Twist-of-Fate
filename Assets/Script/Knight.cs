using UnityEngine;
using System.Collections;

public class Knight : MonoBehaviour
{
    PhysicsManager physManager;

    public float AttackStrength = 200.0f;
    public float Inertia = 400.0f;

    public float TimeOutOfAlert = 5.0f;
    public float TimeAlertBack = 3.0f;
    public float TimeBeforeAttack = 0.75f;

    float strength;
    float timerOutOfAlert;
    float timerAlertBack;
    float timerBeforeAttack;

    StateManager.State State
    {
        get { return physManager.State; }
        set { physManager.State = value; }
    }

    void Start()
    {
        physManager = GetComponent<PhysicsManager>();
    }

    void FixedUpdate()
    {
        switch (State)
        {
            case StateManager.State.Stand:
                physManager.speed.x = 0;
                if (physManager.CheckEnemyAround())
                {
                    timerOutOfAlert = 0;
                    State = StateManager.State.Alert;
                }
                else if (physManager.CheckNearWall())
                    State = StateManager.State.Turn;
                break;
            case StateManager.State.Walk:
                if (physManager.CheckEnemyAround())
                {
                    timerOutOfAlert = 0;
                    State = StateManager.State.Alert;
                }
                else if (physManager.CheckNearWall())
                {
                    State = StateManager.State.Turn;
                }
                break;
            case StateManager.State.Turn:
                physManager.Direction = !physManager.Direction;
                physManager.State = StateManager.State.Walk;
                break;
            case StateManager.State.Alert:
                physManager.speed.x = (physManager.Direction ? physManager.WalkSpeed : -physManager.WalkSpeed) / 2.0f;
                if (physManager.CheckEnemyNear())
                {
                    timerBeforeAttack = 0;
                    State = StateManager.State.PrepareAttack;
                }
                else if (physManager.CheckEnemyAround())
                {
                    timerOutOfAlert = 0;
                }
                else
                {
                    timerOutOfAlert += Time.deltaTime;
                    if (timerOutOfAlert >= TimeOutOfAlert)
                        State = StateManager.State.Walk;
                }
                break;
            case StateManager.State.AlertBack:
                physManager.speed.x = (physManager.Direction ? physManager.WalkSpeed : -physManager.WalkSpeed) / 2.0f;
                if (physManager.CheckEnemyAround())
                {
                    physManager.speed.x *= -1;
                }
                timerAlertBack += Time.deltaTime;
                if (timerAlertBack >= TimeAlertBack)
                {
                    timerOutOfAlert = 0;
                    State = StateManager.State.Alert;
                }
                break;
            case StateManager.State.PrepareAttack:
                physManager.speed.x = 0;
                timerBeforeAttack += Time.deltaTime;
                if (timerBeforeAttack >= TimeBeforeAttack)
                {
                    strength = AttackStrength;
                    State = StateManager.State.Attack;
                }
                break;
            case StateManager.State.Attack:
                strength -= Inertia * Time.deltaTime;
                if (strength < 0)
                {
                    timerAlertBack = 0;
                    physManager.speed.x = 0;
                    State = StateManager.State.AlertBack;
                }
                else
                    physManager.speed.x = physManager.Direction ? strength : -strength;
                break;
        }
    }
}
