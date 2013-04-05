using UnityEngine;
using System.Collections;

public class TargetCameraManager : MonoBehaviour
{
	Transform[] m_players;
	Transform m_target;
	Transform m_current;

	public bool m_check_bounds;
	public bool m_change_player;
	public int m_iPlayer;
	public float speed;
	
	public float minX;
	public float maxX;
	public float minY;
	public float maxY;
	
	bool bChangePos=false;
	Vector3 newPos;
	
	bool levelCompleted=false;
	
	float smoothTime = 0.3F;
    Vector3 velocity = Vector3.zero;
	float distThreshold = 0.01f;
	
	float smoothTimeP = 0.3F;
    Vector3 velocityP = Vector3.zero;
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Awake()
	{
		m_players = new Transform[2];

		if(GameObject.Find("PLAYER0"))
			m_players[0] = GameObject.Find("PLAYER0").transform;
		else
			m_players[0] = GameObject.Find("PLAYER0_concept").transform;
		
		if(GameObject.Find("PLAYER1"))
			m_players[1] = GameObject.Find("PLAYER1").transform;
		else
			m_players[1] = GameObject.Find("PLAYER1_concept").transform;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Start()
	{
		m_current = m_players[0].transform;
		transform.position = m_current.position;
		levelCompleted=false;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void Update()
	{
		//Update camera position
		if(!levelCompleted)
		{
			//change active player
			if(m_change_player && m_players[m_iPlayer]!=null)
			{
				m_target = m_players[m_iPlayer].transform;
				bChangePos=true;
			}
			//follow the player
			else if(m_current)
				transform.position = Vector3.SmoothDamp(transform.position, m_current.position, ref velocityP, smoothTimeP);
				
			//Player change
			if(bChangePos)
				ChangeActivePlayer();
			
			//check camera limits
			if(m_check_bounds)
				CheckBounds();
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void ChangePlayer()
	{
		m_change_player=true;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void SetPlayerActive(int id)
	{
		m_iPlayer = id;
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void SetLevelCompleted()
	{
		levelCompleted=true;
	} 

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void ChangeActivePlayer()
	{	
		if(m_target!=null)
		{
			transform.position = Vector3.SmoothDamp(transform.position, m_target.position, ref velocity, smoothTime);
		
			float dist = Vector3.Distance(transform.position, m_target.position);
		
			if(dist < distThreshold)
			{
				m_current = m_target;
				bChangePos = false;
				m_change_player = false;
			}
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	void CheckBounds()
	{
		//Check level bounds
		float newX = Mathf.Clamp(transform.position.x, minX, maxX);
		float newY = Mathf.Clamp(transform.position.y, minY, maxY);
		
		transform.position = new Vector3(newX, newY, transform.position.z);
	}
}