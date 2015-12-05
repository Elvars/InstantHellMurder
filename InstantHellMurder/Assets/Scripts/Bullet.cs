using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {
	
	public int damage;
	
	Vector3 startPos;	
	float deathTimer;
	float lifeTime = 10.0f;

	//omat jutut
	float syncTime;
	private float syncDelay = 0f;
	float lastSynchronizationTime = 0f;

	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;
	Rigidbody2D rb;


	
	public override void OnStartServer()
	{
		startPos = transform.position;
		deathTimer = Time.time + lifeTime;
	}


	void Awake()
	{
		rb = gameObject.GetComponent<Rigidbody2D>();

	}


	[ServerCallback]
	void Update()
	{
		if (Time.time > deathTimer)
		{
			deathTimer = 0;
			NetworkServer.Destroy(this.gameObject);
		}


	}


	[ServerCallback]
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (deathTimer == 0)
			return;
		
		bool destroyMe = false;		
		
		PlayerCombat tc = collider.gameObject.GetComponent<PlayerCombat>();
		if (tc != null)
		{
			tc.GotHitByMissile(damage);
			destroyMe = true;
		}
		
		// destroy missile
		if (destroyMe)
		{
			deathTimer = 0;
			NetworkServer.Destroy(this.gameObject);
		}
	}

	[ServerCallback]
	void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.tag=="Ground")
		{
			deathTimer = 0;
			NetworkServer.Destroy(this.gameObject);
		}
	}


	[ServerCallback]
	void OnEnable()
	{
		//rb.transform.rotation = GameObject.FindGameObjectWithTag("WeaponHinge").transform.rotation;
		rb.AddForce(transform.up * 500f);
		
	}
	


	private void SyncedMovement()
	{
		syncTime += Time.deltaTime;
		
		GetComponent<Rigidbody2D>().position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
	}

}
