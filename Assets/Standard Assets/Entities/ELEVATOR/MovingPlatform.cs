using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

	public Transform[] nodi;
	public float speed;

	public static int ID = -1;
	public int active;

	private Transform t;
	private int currentNode;
	private int nextNode{
		get
		{
			return ((currentNode + 1) % nodi.Length);
		}
	}

	// Use this for initialization
	void Start () {
		currentNode = 1;
		t = this.transform;
	}
	
	void FixedUpdate()
	{
		Vector2 velocity = (nodi [currentNode].position - t.position).normalized * speed * Time.deltaTime;
		if(Vector2.Distance(t.position, nodi [currentNode].position) <= 0.5f)
		{
			velocity = Vector2.ClampMagnitude(velocity, (t.position - nodi[currentNode].position).magnitude);
			currentNode = nextNode;
		}
		t.Translate (velocity);
		if (currentNode == 0)
			t.position = nodi [currentNode].position;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
