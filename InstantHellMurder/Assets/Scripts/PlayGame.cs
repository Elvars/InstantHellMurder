using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayGame : NetworkBehaviour {
	
	static public PlayGame singleton;
	

	public int controlComplete;
	
	[SyncVar]
	bool complete;
	
	void Awake () 
	{
		singleton = this;
	}
	

	
	public static bool GetComplete()
	{
		return singleton.complete;
	}
	


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (isServer)
			{
				NetworkManager.singleton.StopHost();
			}
			else
			{
				NetworkManager.singleton.client.Disconnect();
			}
		}

	}
}
