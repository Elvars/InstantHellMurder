using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {
	
	public static GameManager singleton;
	public int controlComplete;
	
	[SyncVar]
	bool complete;


	void Awake () 
	{
		singleton = this;
		Application.targetFrameRate = 60;
	}
	
	
	public static bool GetComplete()
	{
		return singleton.complete;
	}


	void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			if (isServer)
			{
				CustomNetworkManager.instance.StopHost();
			}
			else
			{
				CustomNetworkManager.instance.client.Disconnect();
			}
		}
		
	}
}
