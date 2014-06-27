using UnityEngine;
using System.Collections;

public class HideSprite : MonoBehaviour
{
    Sprite previousSprite;
    SpriteRenderer spriteRender;

    bool Visible
    {
        get { return spriteRender.sprite != null; }
        set { spriteRender.sprite = value ? previousSprite : null; }
    }

    // Use this for initialization
    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        previousSprite = spriteRender.sprite;
        Visible = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
