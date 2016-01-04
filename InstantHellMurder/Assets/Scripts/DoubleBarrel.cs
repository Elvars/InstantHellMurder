using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class DoubleBarrel : NetworkBehaviour {

	public Transform bulletPoint1;
	public Transform bulletPoint2;

	public Transform weaponHinge;

	public GameObject Bullet;

	void Start()
	{
		bulletPoint1 = gameObject.transform.GetChild(0).GetChild(0).transform;
		bulletPoint2 = gameObject.transform.GetChild(1).GetChild(0).transform;

		weaponHinge = gameObject.transform.parent.gameObject.transform;
	}


	public void Shoot()
	{
		GameObject bullet1 = (GameObject)GameObject.Instantiate(Bullet, bulletPoint1.transform.position, weaponHinge.transform.rotation);
		NetworkServer.Spawn(bullet1);

		GameObject bullet2 = (GameObject)GameObject.Instantiate(Bullet, bulletPoint2.transform.position, weaponHinge.transform.rotation);
		NetworkServer.Spawn(bullet2);
	}
}
