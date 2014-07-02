using UnityEngine;
using System.Collections;

public class Knight : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	
	}

	bool IsEnemyAround()
	{
		Vector2 vStart = new Vector2(0, 0);
		Vector3 vEnd = new Vector3 (0.1f, 0, 0);

		Debug.DrawRay (vStart, vEnd);
		if (Physics2D.Raycast (vStart, vEnd, 0.5f) != null)
		{
			Debug.LogError("SDFSDSFSDFSDFS");
		}
		return false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		IsEnemyAround ();
	}
}
