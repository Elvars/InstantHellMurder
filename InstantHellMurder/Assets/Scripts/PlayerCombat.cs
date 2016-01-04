using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerCombat : NetworkBehaviour
{
	public PlayerType tt;

	public GameObject Bullet;

	public GameObject Weapon;

	private Transform bulletPoint;
	private Transform weaponHinge;

	
	[SyncVar]
	public int health = 100;

	[SyncVar]
	public bool alive = true;
	
	
	void Start()
	{
		weaponHinge = gameObject.transform.GetChild(0);

		Weapon = CustomNetworkManager.instance.Weapons[0];
		GameObject ebin = (GameObject)Instantiate(Weapon, new Vector3(weaponHinge.transform.position.x+0.6f, weaponHinge.transform.position.y, weaponHinge.transform.position.z), weaponHinge.transform.rotation);
		ebin.transform.parent = weaponHinge.transform;
		NetworkServer.Spawn(ebin);
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
			RpcRespawn();
			return;
		}
	}


	[Server]
	public void GotHitByBullet(int damage)
	{
		TakeDamage(damage);
	}


	[Server]
	public void GotHitByHealthPowerUp(int amount)
	{
		TakeHealth(amount);
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


	[Server]
	void TakeHealth(int amount)
	{
		if(!alive)
			return;

		health += amount;
	}


	[Command]
	public void CmdFireTurret()
	{

		if(gameObject.GetComponentInChildren<SingleBarrel>() != null)
		{
			gameObject.GetComponentInChildren<SingleBarrel>().Shoot();
		}
		else
		{
			gameObject.GetComponentInChildren<DoubleBarrel>().Shoot();

		}
	}

	
	[Command]
	public void CmdKillSelf()
	{
		TakeDamage(1000000);
	}


	//problems here
	[Command]
	public void CmdChangeWeapon(int i)
	{
		gameObject.GetComponentInChildren<SingleBarrel>().Destroy();

		Weapon = CustomNetworkManager.instance.Weapons[i];
		GameObject ebin = (GameObject)Instantiate(Weapon, new Vector3(weaponHinge.transform.position.x, weaponHinge.transform.position.y, weaponHinge.transform.position.z), weaponHinge.transform.rotation);
		ebin.transform.parent = weaponHinge.transform;
		NetworkServer.Spawn(ebin);
	}


}
