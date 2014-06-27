using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour
{
    public StateManagement state;
    protected Transform transform;
    private Animator animator;

    void Awake()
    {
        state = GetComponent<StateManagement>();
        animator = this.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        int s;
        switch (state.state)
        {
            case StateManagement.State.Stand:
                s = 0;
                break;
            case StateManagement.State.Walk:
                s = 1;
                break;
            case StateManagement.State.Crouch:
                s = 2;
                break;
            case StateManagement.State.Jump:
                s = 3;
                break;
            case StateManagement.State.Attack:
                s = 4;
                break;
            default:
                s = 0;
                break;
        }
        animator.SetInteger("Stato", s);
    }
}
