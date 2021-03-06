﻿using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{
    public enum Alignment
    {
        BottomLeft,
        BottomRight,
        TopLeft,
        TopRight,
        Centered,
        Stretched
    }

    public bool ShowPlayerHUD;
    public bool ShowEnemyHUD;
    public GameObject PlayerHUD;
    public Alignment PlayerHudAlignment;
    public GameObject EnemyHUD;
    public Alignment EnemyHudAlignment;

    /// <summary>
    /// Quando un oggetto è impostato in questa variabile, la telecamera
    /// si sposta seguendo l'oggetto specificato. Questo fa in modo che
    /// sia sempre al centro dello schermo nell'interno dei bordi. Per
    /// i bordi, vedere le variabili col prefisso Bound*
    /// </summary>
    public GameObject ObjectToFollow;

    /// <summary>
    /// Sguardo temporaneo verso un determinato oggetto
    /// </summary>
    public GameObject ObjectPeek;

    /// <summary>
    /// Quanto tempo per lo sguardo
    /// </summary>
    public float PeekTime = 8.0f;

    /// <summary>
    /// Bordo in alto a sinistra della telecamera
    /// </summary>
    public GameObject BoundTopLeft;

    /// <summary>
    /// Bordo in alto a destra della telecamera
    /// </summary>
    public GameObject BoundTopRight;

    /// <summary>
    /// Bordo in basso a sinistra della telecamera
    /// </summary>
    public GameObject BoundBottomLeft;

    /// <summary>
    /// Bordo in basso a destra della telecamera
    /// </summary>
    public GameObject BoundBottomRight;

    /// <summary>
    /// Sfondo che seguirà la telecamera
    /// </summary>
    public GameObject SpriteBackground;

    /// <summary>
    /// Modalità con cui lo sfondo si applica alla vista della telecamera
    /// </summary>
    public Alignment BackgroundAlignment;

    private float BackgroundBoundsSizeX;
    private float BackgroundBoundsSizeY;
    public float timer;
    private Vector2 prevCoord;
	bool x = false;
    public void setObjectToFollow(GameObject obj)
    {
        timer = 0;
        ObjectToFollow = obj;
    }

    private void AdjustBackground()
    {
        if (SpriteBackground != null)
        {
            float height = Camera.main.orthographicSize;
            float width = height * Camera.main.aspect;
            Vector3 newPos = new Vector3(0, 0, 0);
            Vector3 newScale = new Vector3(1, 1, 1);
            switch (BackgroundAlignment)
            {
                case Alignment.BottomLeft:
                    break;
                case Alignment.BottomRight:
                    break;
                case Alignment.TopLeft:
                    break;
                case Alignment.TopRight:
                    break;
                case Alignment.Centered:
                    newPos = transform.position;
                    break;
                case Alignment.Stretched:
                    newPos = transform.position;
                    newScale.x = width / BackgroundBoundsSizeX * 2;
                    newScale.y = height / BackgroundBoundsSizeY * 2;
                    break;
            }
            newPos.z = -newPos.z;
            SpriteBackground.transform.position = newPos;
            SpriteBackground.transform.localScale = newScale;
        }
    }

    void Start()
    {
        BackgroundBoundsSizeX = SpriteBackground.renderer.bounds.size.x;
        BackgroundBoundsSizeY = SpriteBackground.renderer.bounds.size.y;
        AdjustBackground();

        // prende la posizione X ed Y, salvandosi la Z originale
        if (ObjectToFollow != null)
        {
            transform.position = new Vector3(ObjectToFollow.transform.position.x,
                                      ObjectToFollow.transform.position.y,
                // la z deve rimanere sempre la stessa
                                      this.transform.position.z);
        }
    }

    private enum BorderType
    {
        Left,
        Top,
        Right,
        Bottom
    }
    private float GetBorder(GameObject g1, GameObject g2, BorderType type)
    {
        const float UNREACHABLE_BORDER = 10000.0f;

        if (g1 == null || g2 == null)
        {
            switch (type)
            {
                case BorderType.Left:
                    return -UNREACHABLE_BORDER;
                case BorderType.Top:
                    return UNREACHABLE_BORDER;
                case BorderType.Right:
                    return UNREACHABLE_BORDER;
                case BorderType.Bottom:
                    return -UNREACHABLE_BORDER;
            }
        }
        else
        {
            switch (type)
            {
                case BorderType.Left:
                    return Mathf.Max(g1.transform.position.x, g2.transform.position.x);
                case BorderType.Top:
                    return Mathf.Min(g1.transform.position.y, g2.transform.position.y);
                case BorderType.Right:
                    return Mathf.Min(g1.transform.position.x, g2.transform.position.x);
                case BorderType.Bottom:
                    return Mathf.Max(g1.transform.position.y, g2.transform.position.y);
            }
        }
        // caso non gestito. Non capita praticamente mai, ma disattiva
        // il warning dal compilatore
        return UNREACHABLE_BORDER;
    }

    void Align(GameObject obj, Alignment type)
    {
        if (obj == null)
            return;
        Vector3 pos = transform.position;
        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;
        float objWidth = obj.renderer.bounds.size.x;
        float objHeight = obj.renderer.bounds.size.y;

        Vector3 dstPos = obj.transform.localPosition;
        float softDistanceX = Mathf.Sqrt(Mathf.Sqrt(cameraWidth / 2)) / 2;
        float softDistanceY = Mathf.Sqrt(Mathf.Sqrt(cameraHeight / 2)) / 2;
        switch (type)
        {
            case Alignment.TopLeft:
                dstPos.x = -cameraWidth + softDistanceX;
                dstPos.y = +cameraHeight - softDistanceY;
                break;
            case Alignment.TopRight:
                dstPos.x = +cameraWidth - objWidth - softDistanceX;
                dstPos.y = +cameraHeight - softDistanceY;
                break;
            case Alignment.BottomLeft:
                dstPos.x = -cameraWidth + softDistanceX;
                dstPos.y = -cameraHeight + objHeight + softDistanceY;
                break;
            case Alignment.BottomRight:
                dstPos.x = +cameraWidth - objWidth - softDistanceX;
                dstPos.y = -cameraHeight + objHeight + softDistanceY;
                break;
        }
        obj.transform.localPosition = dstPos;
    }

    float ReachThatPoint(float pSrc, float pDst, float speed)
    {
        if (pSrc > pDst)
        {
            pSrc -= speed * Time.deltaTime;
            if (pSrc < pDst)
                pSrc = pDst;
        }
        else if (pSrc < pDst)
        {
            pSrc += speed * Time.deltaTime;
            if (pSrc > pDst)
                pSrc = pDst;
        }
        else pSrc = pDst;
        return pSrc;
    }

    void Update()
    {
        // Memorizza la vecchia posizione della telcamera, che poi verrà modificata
        Vector3 pos = transform.position;
        float height = Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;

        // verifica inanzitutto che l'oggetto da seguire esista e sia impostato,
        // altrimenti non seguire alcun oggetto
        const float CameraSpeed = 9.6f;
        if (ObjectPeek != null)
        {
            pos.x = ReachThatPoint(pos.x, ObjectPeek.transform.position.x, CameraSpeed);
            pos.y = ReachThatPoint(pos.y, ObjectPeek.transform.position.y, CameraSpeed);
            if (Mathf.Abs(pos.x - prevCoord.x) < 0.01 &&
                Mathf.Abs(pos.y - prevCoord.y) < 0.01)
            {
                timer += Time.deltaTime;
                if (timer >= PeekTime)
                {
                    timer = 0;
                    ObjectPeek = null;
                }
            }
            else
                prevCoord = new Vector2(pos.x, pos.y);
        }
        else if (ObjectToFollow != null)
        {
            pos.x = ReachThatPoint(pos.x, ObjectToFollow.transform.position.x, CameraSpeed);
            pos.y = ReachThatPoint(pos.y, ObjectToFollow.transform.position.y, CameraSpeed);
        }

        // aggiusta i bordi della telecamera
        pos.x = Mathf.Max(pos.x - width, GetBorder(BoundTopLeft, BoundBottomLeft, BorderType.Left)) + width;
        pos.x = Mathf.Min(pos.x + width, GetBorder(BoundTopRight, BoundBottomRight, BorderType.Right)) - width;
        pos.y = Mathf.Min(pos.y + height, GetBorder(BoundTopLeft, BoundTopRight, BorderType.Top)) - height;
        pos.y = Mathf.Max(pos.y - height, GetBorder(BoundBottomLeft, BoundBottomRight, BorderType.Bottom)) + height;

        // aggiorna lo sfondo
        AdjustBackground();

        // inserisce i cambiamenti nella telcamera, spostandola. Questo avviene solo
        // dopo aver fatto tutti i calcoli necessari.
        transform.position = pos;

        // stampa le HUD
        if (ShowPlayerHUD)
            Align(PlayerHUD, PlayerHudAlignment);
        if (ShowEnemyHUD)
            Align(EnemyHUD, EnemyHudAlignment);

		if(! x && TRAP.IsBroken){
			x = true;
			AudioClip combat = (AudioClip)Resources.Load("Twist Of Fate Combattimento");
			audio.clip = combat;
			audio.Play();
			audio.volume = 0.15f;
			
		}
    }
}