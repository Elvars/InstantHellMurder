using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CameraFollow : NetworkBehaviour {
	
	bool follow = false;

	float syncTime;
	private float syncDelay = 0f;
	float lastSynchronizationTime = 0f;
	
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;


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

	public void MoveToRight()
	{

		//gameObject.transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x+3, transform.position.y, transform.position.z), 3);
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		if (stream.isWriting)
		{
			syncPosition = gameObject.transform.position;
			stream.Serialize(ref syncPosition);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			gameObject.transform.position = syncPosition;
		}
	}
	
	private void SyncedMovement()
	{
		syncTime += Time.deltaTime;
		
		gameObject.transform.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
	}
}
