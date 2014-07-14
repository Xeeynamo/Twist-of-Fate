using UnityEngine;
using System.Collections;

public class LanciaProiettili : MonoBehaviour
{
    public GameObject Bullet;
    /// <summary>
    /// Tempo che intercorre tra un proiettile e l'altro
    /// </summary>
    public float TimeShoot = 0f;
    /// <summary>
    /// Velocità del singolo proiettile
    /// </summary>
    public float Velocità = 6;
    // Use this for initialization

    private bool Direzione
    {
        get { return transform.rotation.y != 0; }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TimeShoot -= Time.deltaTime;
        if (TimeShoot < 0 && Attiva())
        {
			audio.Play();
            GameObject istance = (GameObject)Instantiate(Bullet, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), transform.rotation);
            MuoviProiettile bullet = istance.GetComponent<MuoviProiettile>();
            bullet.Velocità = Direzione ? +Velocità : -Velocità;
            TimeShoot = 1.2f;
        }
    }

	bool Attiva (){
		Debug.DrawRay (new Vector2 (this.transform.position.x, this.transform.position.y), Direz() * 3.5f, Color.red);
		return(Physics2D.Raycast (new Vector2 (this.transform.position.x, this.transform.position.y), Direz(), 4f, 1 << 13));

	}
	Vector3 Direz(){
		if (Direzione)
						return(Vector3.right);
				else
						return (Vector3.left);

	}
}
