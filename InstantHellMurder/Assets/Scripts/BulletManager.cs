using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class BulletManager : NetworkBehaviour {
	
	Rigidbody2D rb;
	
	void Awake()
	{
		rb = gameObject.GetComponent<Rigidbody2D>();
	}
	
	void OnEnable()
	{
		rb.transform.rotation = GameObject.FindGameObjectWithTag("WeaponHinge").transform.rotation;
		rb.AddForce(transform.up * 300f);
		Invoke("Destroy", 5f);
	}
	
	void Start () 
	{
		
	}

	void Destroy()
	{
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}