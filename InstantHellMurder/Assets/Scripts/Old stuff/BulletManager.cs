using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class BulletManager : NetworkBehaviour {
	
	Rigidbody2D rb;

	float syncTime;
	private float syncDelay = 0f;
	float lastSynchronizationTime = 0f;

	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;
	
	void Awake()
	{
		rb = gameObject.GetComponent<Rigidbody2D>();
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		CancelInvoke();
		if(collision.gameObject.tag=="Ground")
		{
			gameObject.SetActive(false);
		}
	}
	
	void OnEnable()
	{
		rb.transform.rotation = GameObject.FindGameObjectWithTag("WeaponHinge").transform.rotation;
		Invoke("Destroy", 10f);
		rb.AddForce(transform.up * 500f);

	}

	void OnDisable()
	{
		CancelInvoke();
	}


	void Update()
	{
		//SyncedMovement();
	}


	void Destroy()
	{
		gameObject.SetActive(false);
	}


	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		if (stream.isWriting)
		{
			syncPosition = rb.position;
			stream.Serialize(ref syncPosition);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			rb.position = syncPosition;
		}
	}

	private void SyncedMovement()
	{
		syncTime += Time.deltaTime;
		
		GetComponent<Rigidbody2D>().position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
	}

	

}