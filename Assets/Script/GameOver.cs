using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour
{
    public float TimeFade = 1.0f;
    public GameObject fade;

    public enum State
    {
        Opening,
        Waiting,
        Exiting,
    }

    public State state;
    public float timer = 1.0f;
    bool canSkip = false;

    void SetFadeAlpha(float alpha)
    {
        if (fade != null)
            fade.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, alpha);
    }
    void Start()
    {
        state = State.Opening;
        timer = 1.0f;
        fade.transform.position = new Vector3(0.0f, 0.0f, -8.0f);
        SetFadeAlpha(timer);
    }

    void Update()
    {
        switch (state)
        {
            case State.Opening:
                timer -= TimeFade * Time.deltaTime;
                if (timer < 0.5f)
                    canSkip = true;
                if (timer <= 0.0f)
                {
                    timer = 0.0f;
                    state = State.Waiting;
                }
                SetFadeAlpha(timer);
                break;
            case State.Waiting:
                break;
            case State.Exiting:
                timer += TimeFade * Time.deltaTime;
                if (timer > 1.0f)
                {
                    Application.LoadLevel(2);
                    timer = 1.0f;
                }
                SetFadeAlpha(timer);
                break;
        }

        if (canSkip == true && Input.anyKey)
            state = State.Exiting;
    }
}
