using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {
	
	private float deathTimer;
	private float lifeTime = 10.0f;

	[SyncVar] 
	Vector3 pos;
	
	private Rigidbody2D rb;

	public int damage;


	
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

		pos = gameObject.transform.position;

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
		
		PlayerCombat pc = collision.gameObject.GetComponent<PlayerCombat>();
		if (pc != null)
		{
			pc.GotHitByBullet(damage);
			destroyMe = true;
		}
		
		// destroy bullet
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
