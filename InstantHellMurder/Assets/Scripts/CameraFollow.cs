using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CameraFollow : NetworkBehaviour {
	
	public bool follow = false;

	float syncTime;
	private float syncDelay = 0f;
	
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;


	void Start()
	{
		follow = true;
	}
	
	void FixedUpdate()
	{

		if(follow && isLocalPlayer)
		{
			//gameObject.transform.position = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.y+1, GameObject.FindGameObjectWithTag("Player").transform.position.y+1, -10);

			gameObject.transform.position = new Vector3(transform.position.x, GameObject.FindGameObjectWithTag("Player").transform.position.y+1, transform.position.z);
			gameObject.transform.position = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, transform.position.y+1, transform.position.z);
		}
		else
		{
			SyncedMovement();
		}

	}
	
	
	public void SetTrue()
	{
		follow = true;
	}
	

	private void SyncedMovement()
	{
		syncTime += Time.deltaTime;
		
		gameObject.transform.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
	}
}
