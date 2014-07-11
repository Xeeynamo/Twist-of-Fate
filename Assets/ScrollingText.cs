using UnityEngine;
using System.Collections;

public class ScrollingText : MonoBehaviour {
	
	
	public int caratteriAlSecondo = 8;
	public string[] showText;
	public string NomeGiocatore;
	public int areaWidth;
	public int areaHeight;
	public GUITexture imgStory;
	public Texture[] imgList;
	
	private float secondiPerOgniCarattere;
	private float timer = 0f;
	private int curChar;
	private int actText;
	private bool inputName = false;


	public Texture2D storyBg;
	public Texture2D nameBg;

	public string storyText;
	public float screenXT, screenYT; 
	public int areaWidthT, areaHeightT;


	// Use this for initialization
	void Start () { 
		NomeGiocatore = "Richard";
		showText = new string[8];
		curChar = 0;
		secondiPerOgniCarattere = 1.0f/caratteriAlSecondo;
		imgStory.texture = imgList[0];
		actText = 0;
		showText[0] = "Impero di Heisenfall - 21° anno dalla grande unificazione\nLa guerra che portò i 12 regni della congregazione di Heisen sotto una sola corona era orami un lontano e triste ricordo.";
		showText[1] = "La ricostruzione procedeva per il meglio in quasi tutti regni, solo uno faceva tristemente eccezione:\nl'undicesimo regno, Artemir"; 
		showText[2] = "Ad Artemir non solo la ricostruzione non procedeva ma, in alcuni punti, la popolazione viveva in condizioni ben peggiori di quelle vissute nel recente conflitto.\nLa colpa di questa situazione è da attribuire all'attuale govarnante che, da pieno sostenitore dell'impero, diventò con il passare del tempo sempre più ostile \nall'imperatore.";
        showText[3] = "";
        showText[4] = "";
        showText[5] = "";
        showText[6] = "";
        showText[7] = "";

		storyBg = new Texture2D (areaWidthT, areaHeightT);
		for (int i = 0; i < storyBg.width; i++)
		{
			for (int j = 0; j < storyBg.height; j++)
			{
				storyBg.SetPixel (i, j, new Color (0f,0f,0f,0.5f));
			}
		}
		storyBg.Apply ();

		nameBg = new Texture2D (areaWidth, areaHeight);
		for (int i = 0; i < nameBg.width; i++)
		{
			for (int j = 0; j < nameBg.height; j++)
			{
				nameBg.SetPixel (i, j, new Color (0f,0f,0f,0.5f));
			}
		}
		nameBg.Apply ();

	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (actText == 3)
		{
			Time.timeScale = 0;
			inputName = true;
		}
		if(actText == 2)
		{
			imgStory.texture = imgList[1];
		}
		if(actText == 4)
		{
			imgStory.texture = imgList[2];
		}
		if(actText == 5)
		{
			imgStory.texture = imgList[3];
		}
		if(actText == 7)
		{
			imgStory.texture = imgList[4];
		}

		if(actText >= 8)
		{
			Application.LoadLevel(2);
		}
		if(timer > secondiPerOgniCarattere && curChar < showText[actText].Length)
		{
			int actChar = Mathf.FloorToInt(timer/secondiPerOgniCarattere);
			curChar += actChar;
			if(curChar > showText[actText].Length)
				curChar = showText[actText].Length;
			timer -= actChar * secondiPerOgniCarattere;
			storyText = showText[actText].Substring(0, curChar);
		}
		if(Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(0))
		{
			if(curChar == showText[actText].Length)
			{	
				storyText = "";
				timer = 0f;
				actText++;
				curChar = 0;
			}
			else
			{
				storyText = showText[actText];
				curChar = showText[actText].Length;
			}
		}
	}
	
	void OnGUI()
	{
		if(inputName)
		{
			float screenX = ((Screen.width * 0.5f) - (areaWidth * 0.5f)); 
			float screenY = ((Screen.height * 0.5f) - (areaHeight * 0.5f)); 
			GUILayout.BeginArea (new Rect(screenX, screenY, areaWidth, areaHeight), nameBg);
			GUILayout.Label ("Prego inserire un nome per il protagonista");
			NomeGiocatore = GUILayout.TextField (NomeGiocatore, 15);
			if (GUILayout.Button("Conferma"))
			{
				PlayerPrefs.SetString("Player_Name", NomeGiocatore);
				showText [4] = "Il sovrano prese in mano la situazione mandando un proprio emissario a prendere in mano la situazione deponento l'inetto governante.\nVenne scelto dunque un giovane diplomatico tale " + NomeGiocatore + " distintosi ai tempi dell'accademia per la sua intelligenza e per lo spiccato senso di attaccamento all'impero.";
				showText [5] = "Accettato l'incarico " + NomeGiocatore + " si diresse verso Artemir con la piena convizione di adempiere al proprio compito e compiacere l'imperatore\nma una brutta sorpresa lo attendeva a destinazione...";
				showText [6] = "Infatti il governatore decise, come atto finale di disprezzo nei confronti dell'imperatore, di imprigionare il giovane diplomatico con la piena intenzione di GIUSTIZIARLO.";
				showText [7] = NomeGiocatore + " si troverà dunque chiuso nelle prigioni del castello e dovrà trovare una via d'uscita per poter adempiere al compito affidatogli dall'imperatore.\nL'occasione perfetta giunse quando una scossa di terremoto smosse la porta della cella.";
				Time.timeScale = 1;
				inputName = false;
				actText = 4;
				curChar = 0;
			}
			GUILayout.EndArea();
		}
		else if(!inputName && actText < 8)
		{
			float screenX = ((Screen.width * screenXT) - (areaWidthT * 0.5f)); 
			float screenY = ((Screen.height * screenYT) - (areaHeightT * 0.5f)); 
			GUILayout.BeginArea (new Rect(screenX, screenY, areaWidthT, areaHeightT), storyBg);
			GUILayout.Label(storyText);
			GUILayout.EndArea();
		}
	}
}
