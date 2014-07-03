using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour
{
    Transform transform;
    Animator animator;
    public StateManager.State state;

    void Awake()
    {
        animator = this.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        int s;
        switch (state)
        {
            case StateManager.State.Stand:
                s = 0;
                break;
            case StateManager.State.Walk:
                s = 1;
                break;
            case StateManager.State.Crouch:
                s = 2;
                break;
            case StateManager.State.Jump:
                s = 3;
                break;
            case StateManager.State.Attack:
                s = 4;
                break;
            default:
                s = 0;
                break;
        }
        animator.SetInteger("Stato", s);
    }
}
