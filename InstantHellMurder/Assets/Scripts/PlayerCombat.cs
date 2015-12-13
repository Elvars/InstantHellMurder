using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerCombat : NetworkBehaviour
{
	public PlayerType tt;

	public GameObject Bullet;

	private Transform bulletPoint;
	private Transform weaponHinge;

	
	[SyncVar]
	public int health = 100;

	[SyncVar]
	public int ammunitionTurret = 100;
	
	[SyncVar]
	public bool alive = true;
	
	
	void Awake()
	{
		bulletPoint = gameObject.transform.GetChild(0).GetChild(0).GetChild(0);
		weaponHinge = gameObject.transform.GetChild(0);
	}


	[ClientRpc]
	void RpcRespawn()
	{
		int i = Random.Range(0,3);
		transform.position = GameObject.Find("NetworkManager").GetComponent<CustomNetworkManager>().SpawnPoints[i].transform.position;
		alive = true;
		health = 100;
	}

	
	public override void OnStartClient()
	{
		if (NetworkServer.active)
			return;

	}


	[ServerCallback]
	void Update()
	{
		if (!alive)
		{
//			if (Time.time > deathTimer)
//			{
				RpcRespawn();
			//}
			return;
		}
	}


	[Server]
	public void GotHitByMissile(int damage)
	{
		TakeDamage(damage);
	}

	
	[Server]
	void TakeDamage(int amount)
	{
		if (!alive)
			return;
		
		if (health > amount) {
			health -= amount;
		}
		else
		{
			health = 0;
			alive = false;
		}
	}


	[Command]
	public void CmdFireTurret()
	{
//		if (PlayGame.GetComplete())
//			return;
		

		GameObject missile = (GameObject)GameObject.Instantiate(Bullet, bulletPoint.transform.position, weaponHinge.transform.rotation);
		NetworkServer.Spawn(missile);

	}

	
	[Command]
	public void CmdKillSelf()
	{
		TakeDamage(1000000);
	}


}
