using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{
	GameObject gameManagerObj;

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void Awake()
	{
		gameManagerObj = GameObject.Find("_GAMEMANAGER");
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void PlayPortalSound()
	{
		if(audio && !audio.isPlaying && gameManagerObj.GetComponent<GUIManager>().bAudioFx)
			audio.Play();
	}
}