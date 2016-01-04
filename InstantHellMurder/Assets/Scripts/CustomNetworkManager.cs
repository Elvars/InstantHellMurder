using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

	private float timer = 0;
	private int spawnTime;
	private bool spawnHealth;

	public static CustomNetworkManager instance = null; 

	public GameObject HealthPowerUpObject;

	public List <Transform> SpawnPoints = new List<Transform>();
	public List <Transform> HealthSpawnPoints = new List<Transform>();
	public List <GameObject> Weapons = new List<GameObject>();


	void Awake()
	{

		if (instance == null)
			instance = this;
		
		else if (instance != this)
			Destroy(gameObject);    
		
		DontDestroyOnLoad(gameObject);

		if(NetworkServer.active)
		{
			spawnHealth = true;

			spawnTime = Random.Range (10, 20);
		}
	}


	void Update()
	{
		if(NetworkServer.active)
		{
			if(timer>=spawnTime && spawnHealth)
			{
				SpawnHealthPowerUp();
				spawnHealth = false;
			}
		}
	}


	void SpawnHealthPowerUp()
	{
		timer  = 0;
		spawnTime = Random.Range(10, 20);
		int i  = Random.Range(0, HealthSpawnPoints.Count);
		Vector3 spawnPoint = HealthSpawnPoints[i].transform.position;

		GameObject healthPowerUp = (GameObject)Instantiate(HealthPowerUpObject, spawnPoint, Quaternion.identity);
		NetworkServer.Spawn(healthPowerUp);

		spawnHealth = true;
	}	


	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		int y = Random.Range(0, SpawnPoints.Count);
		
		Vector3 spawnPoint = SpawnPoints[y].transform.position;

		GameObject player = (GameObject)Instantiate(playerPrefab, spawnPoint, Quaternion.identity);
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}


	public override void OnServerConnect(NetworkConnection conn)
	{
		Debug.Log(conn.connectionId);
	}


}
