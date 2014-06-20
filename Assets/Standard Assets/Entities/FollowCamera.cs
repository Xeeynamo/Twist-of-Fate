using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{
    public enum BackgroundAlign
    {
        BottomLeft,
        BottomRight,
        TopLeft,
        TopRight,
        Centered,
        Stretched
    }

	/// <summary>
	/// Quando un oggetto è impostato in questa variabile, la telecamera
	/// si sposta seguendo l'oggetto specificato. Questo fa in modo che
	/// sia sempre al centro dello schermo nell'interno dei bordi. Per
	/// i bordi, vedere le variabili col prefisso Bound*
	/// </summary>
	public GameObject ObjectToFollow;
	
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
    public BackgroundAlign BackgroundAlignment;

    public float DbgWidth;
    public float DbgHeight;
    public float DbgBgWidth;
    public float DbgBgHeight;

	void Start ()
	{
	
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
				return Mathf.Max (g1.transform.position.x, g2.transform.position.x);
			case BorderType.Top:
				return Mathf.Min (g1.transform.position.y, g2.transform.position.y);
			case BorderType.Right:
				return Mathf.Min (g1.transform.position.x, g2.transform.position.x);
			case BorderType.Bottom:
				return Mathf.Max (g1.transform.position.y, g2.transform.position.y);
			}
		}
		// caso non gestito. Non capita praticamente mai, ma disattiva
		// il warning dal compilatore
		return UNREACHABLE_BORDER;
	}

	void Update ()
	{
		// Memorizza la vecchia posizione della telcamera, che poi verrà modificata
		Vector3 pos = transform.position;
		float height = Camera.main.orthographicSize;
		float width = height * Camera.main.aspect;

		// verifica inanzitutto che l'oggetto da seguire esista e sia impostato,
		// altrimenti non seguire alcun oggetto
		if (ObjectToFollow != null)
		{
			// prende la posizione X ed Y, salvandosi la Z originale
			pos = new Vector3(ObjectToFollow.transform.position.x,
			                          ObjectToFollow.transform.position.y,
			                  // la z deve rimanere sempre la stessa
			                          this.transform.position.z);
		}

		// aggiusta i bordi della telecamera
		pos.x = Mathf.Max (pos.x - width, GetBorder(BoundTopLeft, BoundBottomLeft, BorderType.Left)) + width;
		pos.x = Mathf.Min (pos.x + width, GetBorder(BoundTopRight, BoundBottomRight, BorderType.Right)) - width;
		pos.y = Mathf.Min (pos.y + height, GetBorder(BoundTopLeft, BoundTopRight, BorderType.Top)) - height;
		pos.y = Mathf.Max (pos.y - height, GetBorder(BoundBottomLeft, BoundBottomRight, BorderType.Bottom)) + height;

        if (SpriteBackground != null)
        {
            float bgHeight = SpriteBackground.renderer.bounds.size.x;
            float bgWidth = SpriteBackground.renderer.bounds.size.y;
            Vector3 newPos = new Vector3(0, 0, 0);
            Vector3 newScale = new Vector3(1, 1, 1);
            switch (BackgroundAlignment)
            {
                case BackgroundAlign.BottomLeft:
                    break;
                case BackgroundAlign.BottomRight:
                    break;
                case BackgroundAlign.TopLeft:
                    break;
                case BackgroundAlign.TopRight:
                    break;
                case BackgroundAlign.Centered:
                    break;
                case BackgroundAlign.Stretched:
                    newPos = transform.position;
                    //newScale.x = width / bgWidth;
                    newScale.y = height / bgHeight;
                    break;
            }
            SpriteBackground.transform.position = newPos;
            //SpriteBackground.transform.localScale = newScale;
            DbgWidth = width;
            DbgHeight = height;
            DbgBgWidth = bgWidth;
            DbgBgHeight = bgHeight;
        }

		// inserisce i cambiamenti nella telcamera, spostandola. Questo avviene solo
		// dopo aver fatto tutti i calcoli necessari.
		transform.position = pos;
	}
}
