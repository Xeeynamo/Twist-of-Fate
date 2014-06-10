using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{
	public GameObject objectToFollow;

	void Start ()
	{
	
	}

	void Update ()
	{
		// verifica inanzitutto che l'oggetto da seguire esista e sia impostato
		if (objectToFollow != null)
		{
			// prende la posizione X ed Y, salvandosi la Z originale
			Vector3 pos = new Vector3(objectToFollow.transform.position.x,
			                          objectToFollow.transform.position.y,
			                          this.transform.position.z);
			transform.position = pos;
		}
	}
}
