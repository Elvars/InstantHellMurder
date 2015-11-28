using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MainCameraScript : MonoBehaviour {

	bool follow = false;

	void Update()
	{

		if(follow)
		{
			gameObject.transform.position = new Vector3(transform.position.x, GameObject.FindGameObjectWithTag("Player").transform.position.y+1, transform.position.z);
			gameObject.transform.position = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, transform.position.y+1, transform.position.z);
	
		}
	}


	public void ChangeParent()
	{
		if(GetComponent<NetworkView>().isMine){
			//GetComponent(Camera).enabled = true;
			GetComponent<Camera>().enabled= true;
		}
		else{
			//GetComponent(Camera).enabled = false;
			GetComponent<Camera>().enabled= false;
			
		}

		follow = true;


	}
	

}
