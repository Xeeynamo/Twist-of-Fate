using UnityEngine;
using System.Collections;

public class Knight : MonoBehaviour
{
	private Transform trs;
    private int playerMask = 1 << 6;

	// Use this for initialization
	void Start ()
	{
		trs = this.transform;
	}

	bool IsEnemyAround()
	{
		Vector2 vStart = new Vector2(trs.position.x, trs.position.y);
		Vector3 dir = new Vector3 (trs.rotation.x, 0, 0);

		Debug.DrawRay (vStart, Vector3.left*0.5f,Color.blue);
        if (Physics2D.Raycast(vStart, Vector3.left, 0.5f, playerMask) != null)
		{
			//Debug.LogError("SDFSDSFSDFSDFS");
		}
		return false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		IsEnemyAround ();
	}
}
