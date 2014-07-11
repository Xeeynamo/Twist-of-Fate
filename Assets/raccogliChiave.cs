using UnityEngine;
using System.Collections;

public class raccogliChiave : MonoBehaviour {

	private GameObject player;
	public GameObject serratura;

    public float MinAlpha = 0.15f;
    public float MaxAlpha = 0.50f;
    float alphaValue = 1.0f;
    float alphaMul = -4.0f;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
        alphaValue += alphaValue * alphaMul * alphaValue * alphaValue * Time.deltaTime;
        if (alphaValue <= MinAlpha)
        {
            alphaValue = MinAlpha;
            alphaMul *= -1.0f;
        }
        else if (alphaValue >= MaxAlpha)
        {
            alphaValue = MaxAlpha;
            alphaMul *= -1.0f;
        }
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, alphaValue);

		if(Vector2.Distance(player.GetComponent<Transform>().position, transform.position) < 0.5 && Input.GetKeyDown(KeyCode.X))
		{
			serratura.GetComponent<apriBotola>().chiavePresa = true;
			Destroy(gameObject);
		}
	}
}
