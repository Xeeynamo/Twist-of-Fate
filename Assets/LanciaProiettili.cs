using UnityEngine;
using System.Collections;

public class LanciaProiettili : MonoBehaviour {
	public GameObject Bullet;
	float timeShoot = 1.2f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
				timeShoot -= Time.deltaTime;
				if (timeShoot < 0) {
						GameObject istance = (GameObject)Instantiate (Bullet, new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z), transform.rotation);	
			timeShoot = 1.2f;		
		}
	}
}
