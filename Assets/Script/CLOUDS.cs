using UnityEngine;
using System.Collections;

public class CLOUDS : MonoBehaviour
{
    public GameObject objBegin;
    public GameObject objEnd;
    float speed;

    float RandomSpeed()
    {
        return (float)Random.Range(8, 50) / 100.0f;
    }

    void Start()
    {
        speed = RandomSpeed();
    }

    void Update()
    {
        Vector3 pos = transform.position;
        if (pos.x < objEnd.transform.position.x)
        {
            speed = RandomSpeed();
            pos.x = objBegin.transform.position.x;
        }
        pos.x -= speed * Time.deltaTime;
        transform.position = pos;
    }
}
