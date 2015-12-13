using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {
	
	public int damage;
	
	float deathTimer;
	float lifeTime = 10.0f;


	Rigidbody2D rb;


	
	public override void OnStartServer()
	{
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
	void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.tag=="Ground")
		{
			deathTimer = 0;
			NetworkServer.Destroy(this.gameObject);
		}

		if (deathTimer == 0)
			return;
		
		bool destroyMe = false;		
		
		PlayerCombat tc = collision.gameObject.GetComponent<PlayerCombat>();
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
	void OnEnable()
	{
		rb.AddForce(transform.up * 500f);
	}
	


}
