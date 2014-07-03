using UnityEngine;
using System.Collections;

public class EntityManagement : MonoBehaviour
{
    PhysicsManager physManager;

    // Use this for initialization
    void Start()
    {
        physManager = GetComponent<PhysicsManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        physManager.State = StateManager.getStateFromInput(physManager);
    }
}
