using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour
{
	public GUISkin m_skin;
	public string gui_state;
	
	//Mobile->Tamaño de pantalla de referencia: 800x480
	float screen_width = 800.0f;
	//float screen_height = 480.0f;
	
	float screen_ratio;
	
	int buttonSize;
	int buttonSize2;
	int buttonSize3;
	
	int labelSize;
	int marginTitle;
	int marginButton;
	int marginLabel;
	int marginStars;
	
	int black_width;
	int completed_width;
	int completed_height;
	
	string[] player_buttons;
	
	//FPS
	float updateInterval = 1.0f;
	float accum = 0; // FPS accumulated over the interval
	int frames = 0;  // Frames drawn over the interval
	float timeleft;  // Left time for current interval
	string sFPS;
	public bool bFPS = true;
	
	GameManager gameManager;
	ScoreManager scoreManager;
	
	public bool bAudioFx = true;
	bool bAudioFxOld = true;
	
	public Texture2D m_tex_help;
	float ratio;
	float ratio_4_3 = 4.0f/3.0f;
	float height;
	float offset;
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		gui_state="in_game";
		
		
		
		timeleft = updateInterval;
		
		player_buttons = new string[]{"player0_button", "player1_button"};
		
		gameManager = GetComponent<GameManager>();
		scoreManager = GetComponent<ScoreManager>();
		
		bAudioFx = (PlayerPrefs.GetInt("musicFx") == 1);
		bAudioFxOld = bAudioFx;
		
		ratio = (float)Screen.width/(float)Screen.height;
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		////////////////////////////
		//UNA VEZ TERMINADA LA FASE DE PRUEBAS HABRA QUE PONER ESTO EN EL AWAKE!!!
		////////////////////////////
		screen_ratio = Screen.width / screen_width;
		
		buttonSize  = (int)(80.0f * screen_ratio);
		buttonSize2 = (int)(100.0f * screen_ratio);
		buttonSize3 = (int)(120.0f * screen_ratio);
		
		labelSize    = (int)(85.0f * screen_ratio);
		marginLabel  = (int)(20.0f * screen_ratio);
		marginTitle  = (int)(200.0f * screen_ratio);
		marginButton = (int)(50.0f * screen_ratio);
		marginStars  = (int)(20.0f * screen_ratio);
		
		black_width  = (int)(400.0f * screen_ratio);
		completed_width  = (int)(300.0f * screen_ratio);
		completed_height = (int)(37.5f * screen_ratio);
		////////////////////////////
		
		if(bFPS)
			ShowFPS();
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//No usamos el escalado de GUI con GUI.matrix porque estira los controles entre 16/9 y 4/3.
	//Tendriamos que poner el mismo escalado en X-Y y para eso mejor lo hacemos manualmente.
	void OnGUI()
	{
		GUI.skin = m_skin;
		
		if(gui_state=="in_game")
		{
			//Menu button
			if(GUI.Button(new Rect(Screen.width-5-buttonSize,5,buttonSize,buttonSize), "", "pause"))
			{
				gui_state="show_menu";
				scoreManager.SendMessage("SetInPause", true);
			}
			
			//Reset level button
			if(GUI.Button(new Rect(Screen.width-10-buttonSize*2,5,buttonSize,buttonSize), "", "reset"))
				gameManager.SendMessage("ResetLevel");
			
			//Change player button
			if(GUI.Button(new Rect(Screen.width-buttonSize2*2-10,Screen.height-buttonSize2-5,buttonSize2,buttonSize2), "", player_buttons[gameManager.iPlayerActive]))
				gameManager.SendMessage("ChangePlayer");
		}
		
		else if(gui_state=="show_menu")
			OnGUIShowMenu();
		
		else if(gui_state=="show_options")
			OnGUIShowOptions();
		
		else if(gui_state=="show_help")
			OnGUIShowHelp();
		
		else if(gui_state=="show_level_completed")
			OnGUILevelCompleted();
			
		if(bFPS)
			GUI.Label(new Rect(20,50,200,50), sFPS, "fps");
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUILevelCompleted()
	{
		//GUI.Box(new Rect(0,0,Screen.width,Screen.height), "", "background");
		GUI.Box(new Rect(Screen.width/2-black_width/2,0,black_width,Screen.height), "", "black");
		GUI.Box(new Rect(Screen.width/2-completed_width/2, Screen.height/2-marginTitle,completed_width,completed_height), "", "level_completed");
		
		//labels
		GUI.BeginGroup(new Rect(Screen.width/2-labelSize-labelSize/2-marginLabel,Screen.height/2-20,labelSize*3+marginLabel*2,labelSize));
		
		GUI.Label(new Rect(0,0,labelSize,labelSize), "TIME BONUS", scoreManager.bTimeBonus ? "bonus_text_on" : "bonus_text_off");
		GUI.Label(new Rect(labelSize+marginLabel,0,labelSize,labelSize), "PLAYER SWAPS", scoreManager.bChangesBonus ? "bonus_text_on" : "bonus_text_off");
		GUI.Label(new Rect(labelSize*2+marginLabel*2,0,labelSize,labelSize), "SECRET ITEM", scoreManager.bSecretItemBonus ? "bonus_text_on" : "bonus_text_off");
			
		GUI.EndGroup();
		
		//stars
		GUI.BeginGroup(new Rect(Screen.width/2-labelSize-labelSize/2-marginStars,Screen.height/2-100,labelSize*3+marginStars*2,labelSize));
		
		GUI.Box(new Rect(0,0,labelSize,labelSize), "", scoreManager.bTimeBonus ? "star_on" : "star_off");
		GUI.Box(new Rect(labelSize+marginStars,0,labelSize,labelSize), "", scoreManager.bChangesBonus ? "star_on" : "star_off");
		GUI.Box(new Rect(labelSize*2+marginStars*2,0,labelSize,labelSize), "", scoreManager.bSecretItemBonus ? "star_on" : "star_off");
				
		GUI.EndGroup();
		
		//buttons
		GUI.BeginGroup(new Rect(Screen.width/2-buttonSize-buttonSize/2-marginButton,Screen.height-buttonSize-marginButton,buttonSize*3+marginButton*2,buttonSize));
		
		if(GUI.Button(new Rect(0, 0, buttonSize, buttonSize), "", "reset"))
			gameManager.SendMessage("ResetLevel");
		
		if(GUI.Button(new Rect(buttonSize+marginButton, 0, buttonSize, buttonSize), "", "levels"))
			Application.LoadLevel("03_LEVEL_SELECT");
				
		if(GUI.Button(new Rect(buttonSize*2+marginButton*2, 0, buttonSize, buttonSize), "", "next"))
			gameManager.SendMessage("LoadNextLevel");
		
		GUI.EndGroup();
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUIShowMenu()
	{
		GUI.Box(new Rect(-5,-5,Screen.width+10,Screen.height+10), "");
		
		GUI.BeginGroup(new Rect(Screen.width/2-buttonSize3-buttonSize3/2-marginButton,Screen.height/2-buttonSize3/2,buttonSize3*3+marginButton*2,buttonSize3));
		
		if(GUI.Button(new Rect(0,0,buttonSize3,buttonSize3), "", "continue"))
		{
			gui_state = "in_game";
			scoreManager.SendMessage("SetInPause", false);
		}
			
		if(GUI.Button(new Rect(buttonSize3+marginButton,0,buttonSize3,buttonSize3), "", "levels"))
			Application.LoadLevel("03_LEVEL_SELECT");
			
		if(GUI.Button(new Rect(buttonSize3*2+marginButton*2,0,buttonSize3,buttonSize3), "", "options"))
			gui_state = "show_options";
		
		GUI.EndGroup();
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUIShowOptions()
	{	
		GUI.Box(new Rect(0,0,Screen.width,Screen.height), "");
		
		GUI.BeginGroup(new Rect(Screen.width/2-buttonSize3-buttonSize3/2-marginButton,Screen.height/2-buttonSize3/2,buttonSize3*3+marginButton*2,buttonSize3));
		
		bAudioFx = GUI.Toggle(new Rect(0,0,buttonSize3,buttonSize3), bAudioFx, "", "fx");
		
		if(GUI.Button(new Rect(buttonSize3+marginButton,0,buttonSize3, buttonSize3), "", "help"))
			gui_state = "show_help";
		
		GUI.Button(new Rect(buttonSize3*2+marginButton*2,0,buttonSize3, buttonSize3), "", "info");
		
		GUI.EndGroup();
		
		if(GUI.Button(new Rect(20,Screen.height-buttonSize-20,buttonSize, buttonSize), "", "back"))
			gui_state = "show_menu";
		
		if(bAudioFx != bAudioFxOld)
		{
			bAudioFxOld = bAudioFx;
			PlayerPrefs.SetInt("musicFx", bAudioFx ? 1 : 0);
			
			gameManager.SendMessage("SetAudioFx", bAudioFx);
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void OnGUIShowHelp()
	{
		height = ratio_4_3/ratio;
		offset = (1.0f-height)/2.0f;
		GUI.DrawTextureWithTexCoords(new Rect(0,0,Screen.width,Screen.height), m_tex_help, new Rect(0,offset,1,height));
		
		if(GUI.Button(new Rect(20,20,buttonSize, buttonSize), "", "back"))
			gui_state = "show_options";
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void LevelCompleted()
	{
		gui_state="show_level_completed";
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void ShowFPS()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		++frames;

		if (timeleft <= 0.0)
		{
			float fps = accum / frames;
			sFPS = System.String.Format("{0:F2} FPS", fps);

			timeleft = updateInterval;
			accum = 0.0F;
			frames = 0;
		}
	}
}
