using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class NetworkManageri : NetworkBehaviour
{
	private const string typeName = "UniqueGameName";
	private const string gameName = "RoomName";
	
	private bool isRefreshingHostList = false;
	private HostData[] hostList;


	public List <Transform> SpawnPoints = new List<Transform>();
	public GameObject playerPrefab;

	public GameObject bullet;




	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
				StartServer();
			
			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
				RefreshHostList();
			
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
						JoinServer(hostList[i]);
				}
			}
		}
	}
	
	private void StartServer()
	{
		Network.InitializeServer(5, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
		Camera.main.enabled=false;
	}
	
	void OnServerInitialized()
	{
		SpawnPlayer();
	}
	
	
	void Update()
	{
		if (isRefreshingHostList && MasterServer.PollHostList().Length > 0)
		{
			isRefreshingHostList = false;
			hostList = MasterServer.PollHostList();
		}
	}
	
	private void RefreshHostList()
	{
		if (!isRefreshingHostList)
		{
			isRefreshingHostList = true;
			MasterServer.RequestHostList(typeName);
		}
	}
	
	
	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
		Camera.main.enabled=false;
		ClientScene.RegisterPrefab(bullet);

	}
	
	void OnConnectedToServer()
	{
		SpawnPlayer();

	}
	
	
	private void SpawnPlayer()
	{

		int y = Random.Range(0, SpawnPoints.Count);

		Vector3 spawnPoint = SpawnPoints[y].transform.position;

		Network.Instantiate(playerPrefab, spawnPoint, Quaternion.identity, 0);
	}
	
}
