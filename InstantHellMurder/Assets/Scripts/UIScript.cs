#if ENABLE_UNET
using UnityEngine.UI;
namespace UnityEngine.Networking
{
	[AddComponentMenu("Network/NetworkManagerHUD")]
	[RequireComponent(typeof(NetworkManager))]
	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	public class UIScript : MonoBehaviour
	{
		public NetworkManager manager;
		[SerializeField] public Button Host;
		[SerializeField] public Button Join;
		[SerializeField] public GameObject UIPanel;
		//[SerializeField] public Labe ayy; 
		
		// Runtime variable
		
		void Awake()
		{
			manager = GetComponent<NetworkManager>();

			Host.onClick.AddListener (() => {StartHost();});
			Join.onClick.AddListener (() => {StartClient ();});
			//CategoriesStaffButton.onClick.AddListener (() => {ShowStaff ();});
		}
		
		void Update()
		{
			if (!NetworkClient.active && !NetworkServer.active)
			{
				if (Input.GetKeyDown(KeyCode.H))
				{
					StartHost();
				}
				if (Input.GetKeyDown(KeyCode.C))
				{
					StartClient();
				}
			}
			if (NetworkServer.active && NetworkClient.active)
			{
				if (Input.GetKeyDown(KeyCode.X))
				{
					manager.StopHost();
				}
			}
		}

		public void StartHost()
		{

			manager.StartHost();

			ClientScene.Ready(manager.client.connection);



			UIPanel.SetActive(false);


			
			if (ClientScene.localPlayers.Count == 0)
			{
				ClientScene.AddPlayer(0);
			}

		}

		public void StartClient()
		{

			manager.StartClient();


			ClientScene.Ready(manager.client.connection);


			ClientScene.AddPlayer(0);
		}


//		void OnGUI()
//		{
//
//			int xpos = 10;
//			int ypos = 40;
//			int spacing = 24;
//			
//			if (!NetworkClient.active && !NetworkServer.active)
//			{
//			
//			}
//			else
//			{
//				if (NetworkServer.active)
//				{
//					GUI.Label(new Rect(xpos, ypos, 300, 20), "Server: port=" + manager.networkPort);
//				}
//				if (NetworkClient.active)
//				{
//					GUI.Label(new Rect(xpos, ypos, 300, 20), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort);
//				}
//			}
//			
//			if (NetworkClient.active && !ClientScene.ready)
//			{
//				if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Client Ready"))
//				{
//					ClientScene.Ready(manager.client.connection);
//					
//					if (ClientScene.localPlayers.Count == 0)
//					{
//						ClientScene.AddPlayer(0);
//					}
//				}
//			}
//			
//			if (NetworkServer.active || NetworkClient.active)
//			{
//				if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Stop (X)"))
//				{
//					manager.StopHost();
//				}
//			}
//			
//
//		}
	}
};
#endif //ENABLE_UNET