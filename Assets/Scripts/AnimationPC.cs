using UnityEngine;
using System.Collections;

public class AnimationPC : MonoBehaviour
{
	MegaPointCache megaPointCache;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		megaPointCache = GetComponent<MegaPointCache>();
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Activate()
	{
		megaPointCache.animated=true;
		megaPointCache.time=0.0f;
	}
}
