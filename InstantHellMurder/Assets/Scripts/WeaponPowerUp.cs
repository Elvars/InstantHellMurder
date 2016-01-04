using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WeaponPowerUp : NetworkBehaviour {

	public int Health;
	
	private float deathTimer;
	private int deathTime;
	bool destroyMe = false;	


	[ServerCallback]
	void OnTriggerEnter2D(Collider2D collider)
	{
		
		PlayerCombat pc = collider.gameObject.GetComponent<PlayerCombat>();
		if (pc != null)
		{
			pc.CmdChangeWeapon(1);
		}
		
		// destroy powerup
		if (destroyMe)
		{
			deathTimer = 0;
			NetworkServer.Destroy(this.gameObject);
		}
	}
}
