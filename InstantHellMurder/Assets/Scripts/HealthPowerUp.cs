using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class HealthPowerUp : NetworkBehaviour {

	public int Health;

	private float deathTimer;
	private int deathTime;
	bool destroyMe = false;	


	void Start () 
	{
	
	}


	void Update () 
	{

	}


	[ServerCallback]
	void OnTriggerEnter2D(Collider2D collider)
	{
		PlayerCombat pc = collider.gameObject.GetComponent<PlayerCombat>();
		if (pc != null)
		{
			pc.GotHitByHealthPowerUp(Health);
			destroyMe = true;
		}
		
		// destroy powerup
		if (destroyMe)
		{
			deathTimer = 0;
			NetworkServer.Destroy(this.gameObject);
		}
	}
}
