using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {
	
	public static GameManager instance = null; 


	void Awake()
	{
		Application.targetFrameRate = 60;
		
		if (instance == null)
			instance = this;
		
		else if (instance != this)
			Destroy(gameObject);    
		
		DontDestroyOnLoad(gameObject);
	}
}
