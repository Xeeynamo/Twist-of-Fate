using UnityEngine;
using System.Collections;

public class GameOverSprite : MonoBehaviour
{
    public float TimerToShowUp = 2.0f;
    float alpha = 0.0f;

    void SetColor(float alpha)
    {
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, alpha);
    }

    void Start()
    {
        SetColor(alpha);
    }

    void FixedUpdate()
    {
        if (alpha < 1.0f)
            alpha += (1.0f / TimerToShowUp) * Time.deltaTime;
        if (alpha > 1.0f)
            alpha = 1.0f;
        SetColor(alpha);
    }
}
