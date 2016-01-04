using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class SingleBarrel : MonoBehaviour {

	public Transform bulletPoint1;
	
	public Transform weaponHinge;
	
	public GameObject Bullet;
	
	void Start()
	{
		
		bulletPoint1 = gameObject.transform.GetChild(0);
		
		weaponHinge = gameObject.transform.parent.gameObject.transform;
	}


	public void Shoot()
	{
		GameObject bullet1 = (GameObject)GameObject.Instantiate(Bullet, bulletPoint1.transform.position, weaponHinge.transform.rotation);
		NetworkServer.Spawn(bullet1);
	}


	public void Destroy()
	{
		NetworkServer.Destroy(this.gameObject);
	}
}
