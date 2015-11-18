using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CameraFollow : NetworkBehaviour {
	
	bool follow = false;

	void Start()
	{
		follow = true;
	}
	
	void FixedUpdate()
	{
		if(follow)
		{
			gameObject.transform.position = new Vector3(transform.position.x, GameObject.FindGameObjectWithTag("Player").transform.position.y+1, transform.position.z);
			gameObject.transform.position = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, transform.position.y+1, transform.position.z);
		}
	}
	
	
	public void SetTrue()
	{
		follow = true;
	}
}
