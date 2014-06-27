using UnityEngine;
using System.Collections;

public class EntityManagement : MonoBehaviour
{
    PhysicsManagement physics;
    AnimationManager animation;

    // Use this for initialization
    void Start()
    {
        physics = GetComponent<PhysicsManagement>();
        animation = this.GetComponent<AnimationManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }
}
