using UnityEngine;
using System.Collections;

public class LanciaProiettili : MonoBehaviour
{
    public GameObject Bullet;
    public float TimeShoot = 1.2f;
    public float Velocità = 4;
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
        if (TimeShoot < 0)
        {
            GameObject istance = (GameObject)Instantiate(Bullet, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), transform.rotation);
            MuoviProiettile bullet = istance.GetComponent<MuoviProiettile>();
            bullet.Velocità = Direzione ? +Velocità : -Velocità;
            TimeShoot = 1.2f;
        }
    }
}
