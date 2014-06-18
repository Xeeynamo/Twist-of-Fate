using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{
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

	enum BorderType
	{
		Left,
		Top,
		Right,
		Bottom
	}

	void Start ()
	{
	
	}

	float GetBorder(GameObject g1, GameObject g2, BorderType type)
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

		// inserisce i cambiamenti nella telcamera, spostandola. Questo avviene solo
		// dopo aver fatto tutti i calcoli necessari.
		transform.position = pos;
	}
}
