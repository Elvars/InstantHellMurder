using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

	public List <Transform> SpawnPoints = new List<Transform>();



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
