using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerCombat : NetworkBehaviour
{
	public PlayerType tt;

	public GameObject Bullet;

	private Transform bulletPoint;
	private Transform weaponHinge;
	
	public delegate void TakeDamageDelegate(int damage);
	public delegate void DieDelegate();
	public delegate void RespawnDelegate();
	
	[SyncEvent(channel=1)]
	public event TakeDamageDelegate EventTakeDamage;
	
	[SyncEvent]
	public event DieDelegate EventDie;
	
	[SyncEvent]
	public event RespawnDelegate EventRespawn;
	
	[SyncVar]
	public int health = 100;

	[SyncVar]
	public int ammunitionTurret = 100;
	
	[SyncVar]
	public bool alive = true;

	[SyncVar]
	public string playerType;
	

	
	float fireTurretTimer;	

	float deathTimer;

	void Awake()
	{
		bulletPoint = gameObject.transform.GetChild(0).GetChild(0).GetChild(0);
		weaponHinge = gameObject.transform.GetChild(0);
	}
	
	[Server]
	void Respawn()
	{
		InitializeFromTankType(tt);
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		alive = true;
		EventRespawn();
	}
	
	[Server]
	public void InitializeFromTankType(PlayerType newTT)
	{
		tt = newTT;
		playerType = tt.playerTypeName;
		health = tt.maxHealth;
		ammunitionTurret = tt.maxAmmunitionTurret;

		

	}
	
	public override void OnStartClient()
	{
		if (NetworkServer.active)
			return;
		
		//TODO
//		PlayerType found = TankTypeManager.Lookup(tankType);
//		tt = found;
		

	}
	
	[ServerCallback]
	void Update()
	{
		if (!alive)
		{
			if (Time.time > deathTimer)
			{
				Respawn();
			}
			return;
		}
		

	}
	
	public bool CanFireTurret()
	{

		
		if (ammunitionTurret <= 0)
			return false;
		
		if (Time.time < fireTurretTimer)
			return false;
		
		if (!alive)
			return false;
		
		return true;
	}
	

	
	[Server]
	public void GotHitByMissile(int damage)
	{
		EventTakeDamage(damage);
			
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

			EventDie();
			deathTimer = Time.time + 5.0f;
		}
	}
	
	[Command]
	public void CmdFireTurret()
	{
		if (PlayGame.GetComplete())
			return;
		
		if (!CanFireTurret())
			return;

		GameObject missile = (GameObject)GameObject.Instantiate(Bullet, bulletPoint.transform.position, weaponHinge.transform.rotation);

		//missile.GetComponent<Missile>().damage = tt.turretDamage;
		NetworkServer.Spawn(missile);

	}

	

	
	[Command]
	public void CmdKillSelf()
	{
		TakeDamage(1000000);
	}
	
	public override void OnStartLocalPlayer()
	{
		//CmdSetName(Manager.singleton.tankName + "-" + tt.tankTypeName);
	}
}
