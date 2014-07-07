using UnityEngine;
using System.Collections;

public class Thorns : MonoBehaviour
{
    public enum State
    {
        Wait,
        Attack,
        Return,
    }

    public State state;
    public float TimerWait = 3.0f;
    public float TimerAttack = 2.0f;
    public float SpeedReturn = 1.0f;
    float posY;
    float height;
    float timer;
    
    float Show()
    {
        if (transform.rotation.x != 0)
            return transform.position.y - height;
        else
            return transform.position.y + height;
    }

    void Start()
    {
        posY = transform.position.y;
        height = GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        if (transform.rotation.x != 0)
            posY += height;
        else
            posY -= height;
        transform.position = new Vector3(transform.position.x, posY, transform.position.z);
        state = State.Wait;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        switch (state)
        {
            case State.Wait:
                pos.y = posY;
                timer += Time.deltaTime;
                if (timer >= TimerWait)
                {
                    timer = 0;
                    state = State.Attack;
                }
                break;
            case State.Attack:
                pos.y = posY + (transform.rotation.x != 0 ? -height : +height);
                timer += Time.deltaTime;
                if (timer >= TimerAttack)
                {
                    timer = 0;
                    state = State.Return;
                }
                break;
            case State.Return:
                float speed = SpeedReturn * Time.deltaTime;
                if (transform.rotation.x != 0)
                {
                    pos.y += speed;
                    if (pos.y > posY)
                    {
                        state = State.Wait;
                    }
                }
                else
                {
                    pos.y -= speed;
                    if (pos.y < posY)
                    {
                        state = State.Wait;
                    }
                }
                break;
        }
        transform.position = pos;
    }
}
