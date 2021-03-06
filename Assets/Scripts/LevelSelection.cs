using UnityEngine;
using System.Collections;

public class LevelSelection : MonoBehaviour
{
	public Texture2D m_tex;
	
	float ratio;
	float ratio_4_3 = 4.0f/3.0f;
	float height;
	float offset;
		
	public GUISkin m_skin_level_select;
	public int num_levels;
	public int num_levels_total;
	
	float screen_width = 800.0f;
	float screen_height = 480.0f;
	
	int buttonSize;
	int marginH;
	int marginV;
	int gridWidth;
	int gridHeight;
	int gridButtonSize;
	
	int nRows = 3;
	int nCols = 3;
	
	GameObject goAudioManager;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		PlayerPrefs.SetInt("num_levels", num_levels);
		
		goAudioManager = GameObject.Find("goAudioManager");
		
		ratio = (float)Screen.width/(float)Screen.height;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Inicializamos las medidas en función del tamaño de pantalla actual
	void Start()
	{
		int iAudio = PlayerPrefs.GetInt("music"); //0=ON, 1=OFF
		if(iAudio==0 && goAudioManager && !goAudioManager.audio.isPlaying)
			goAudioManager.audio.Play();
		
		buttonSize = (int)(Screen.width * (60.0f/screen_width));

		//marginH = (int)(Screen.width * (150.0f/screen_width));
		marginV = (int)(Screen.height * (50.0f/screen_height));

		gridHeight = Screen.height-marginV*2;
		gridWidth = gridHeight;

		marginH = (int)((Screen.width - gridWidth) / 2f);

		gridButtonSize = (int)(gridWidth/(nCols));
		
		if(!PlayerPrefs.HasKey("first_time"))
			InitializeLevels();
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void InitializeLevels()
	{
		PlayerPrefs.SetString("LEVEL_1_locked", "false");
		PlayerPrefs.SetInt("LEVEL_1_stars", 0);
		
		for(int i=2;i<=num_levels_total;i++)
		{
			PlayerPrefs.SetString("LEVEL_"+ i + "_locked", "true");
			PlayerPrefs.SetInt("LEVEL_"+ i + "_stars", 0);
		}
		
		PlayerPrefs.SetString("first_time", "false");
		
		PlayerPrefs.Save();
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//No podemos usar GUI.SelectionGrid pq no permite poner los textos encima de las imágenes
	void OnGUI()
	{
		GUI.skin = m_skin_level_select;
		
		//GUI.Box(new Rect(0,0,Screen.width, Screen.height), "", "background");
		height = ratio_4_3/ratio;
		offset = (1.0f-height)/2.0f;
		GUI.DrawTextureWithTexCoords(new Rect(0,0,Screen.width,Screen.height), m_tex, new Rect(0,offset,1,height));
			
		GUI.BeginGroup(new Rect(marginH,marginV,gridWidth,gridHeight));
		
		for(int i=0;i<nRows;i++)
		{
			for(int j=0;j<nCols;j++)
			{
				int curr_level = j + i*nCols + 1;
				
				int pX = j*gridButtonSize;
				int pY = i*gridButtonSize;
				
				if(LevelButton(curr_level, pX, pY))
					LoadLevel(curr_level);
			}	
		}
		
		GUI.EndGroup();
		
		if(GUI.Button(new Rect(20,Screen.height-buttonSize-20,buttonSize, buttonSize), "", "back"))
		{	
			Application.LoadLevel("02_MAIN_MENU");
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Dibujamos el botón segun la textura que le corresponda: locked, 0,1,2,3 stars
	bool LevelButton(int curr_lev, int pX, int pY)
	{
		if(PlayerPrefs.GetString("LEVEL_"+ curr_lev + "_locked") == "true")
		{
			GUI.Button(new Rect(pX, pY, gridButtonSize, gridButtonSize), "", "button_locked");
			return false;
		}
		else
		{
			int n_stars = PlayerPrefs.GetInt("LEVEL_"+ curr_lev + "_stars");
			return GUI.Button(new Rect(pX, pY, gridButtonSize, gridButtonSize), curr_lev.ToString(), "button_"+n_stars+"_stars");
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void LoadLevel(int curr_lev)
	{
		string nom = "LEVEL_";
				
		if(curr_lev<10) 
			nom = nom+"0";
		nom = nom + curr_lev;
		
		PlayerPrefs.SetString("next_level", nom);
		Application.LoadLevel("LOADING");
	}
}




