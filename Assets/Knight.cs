using UnityEngine;
using System.Collections;

public class Knight : MonoBehaviour
{
    private int groundMask = 1 << 8;

    PhysicsManager physManager;

    void Start()
    {
        physManager = GetComponent<PhysicsManager>();
    }

    void FixedUpdate()
    {
        switch (physManager.State)
        {
            case StateManager.State.Stand:
                physManager.speedX = 0;
                break;
            case StateManager.State.Walk:
                if (physManager.CheckNearWall())
                    physManager.State = StateManager.State.Turn;
                break;
            case StateManager.State.Turn:
                Debug.Log("Knight turning");
                physManager.Direction = !physManager.Direction;
                physManager.speedX = 0;
                physManager.State = StateManager.State.Walk;
                break;
        }
    }
}
