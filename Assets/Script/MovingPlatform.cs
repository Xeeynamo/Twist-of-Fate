using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

	public Transform[] nodi;
	public float speed;

	private int currentNode;
	private int nextNode
	{
		get
		{
			return ((currentNode + 1) % nodi.Length);
		}
	}

	// Use this for initialization
	void Start () {
		currentNode = 1;
	}
	
	void FixedUpdate()
	{
		Vector2 nodePos = new Vector2 (nodi [currentNode].position.x, nodi [currentNode].position.y);
		Vector2 currentPos = new Vector2 (transform.position.x, transform.position.y);
		Vector2 velocity = (nodePos - currentPos).normalized * speed * Time.deltaTime;
		if(Vector2.Distance(currentPos, nodePos) <= 0.5f)
		{
			velocity = Vector2.ClampMagnitude(velocity, (currentPos - nodePos).magnitude);
			currentNode = nextNode;
		}
		if (currentNode == 0)
			this.transform.position = new Vector3(nodePos.x, nodePos.y, transform.position.z);
		else
			this.transform.Translate (velocity);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
