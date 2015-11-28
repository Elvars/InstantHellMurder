using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	Rigidbody2D rb;
	float syncTime;
	private float syncDelay = 0f;
	float lastSynchronizationTime = 0f;

	
	public enum WeaponRotation
	{
		Up = -90,
		Right = 0,
		Down = 90,
		Left = 180
	}
	
	public WeaponRotation initialRotation;
	public GameObject bullet;

	public int pooledBulletAmount = 40;

	public int jumpCount = 0;
	
	private Vector2 direction;
	private Vector2 mousePos;
	private Transform trans;
	
	private float angle;

	List<GameObject> bullets;

	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;


	void Awake()
	{


		lastSynchronizationTime = Time.time;

		rb = gameObject.GetComponent<Rigidbody2D>();


	}


	void Start()
	{
		trans = gameObject.transform.GetChild(0);

		bullets = new List<GameObject>();

//
		for (int i=0; i<20; i++) 
		{
		
			GameObject obj = (GameObject)Instantiate(bullet);			
			obj.SetActive(false);
			bullets.Add(obj);
		}


	}
	

	// Update is called once per frame
	void Update () 
	{
		if(isLocalPlayer)
		{
			HandleControls();

			HandleRotation();
		}
		
		else
		{
			SyncedMovement();
		}
	}

	void Jump()
	{
		jumpCount++;
		rb.AddForce(Vector2.up*500);

	}

	[Command]
	void CmdShoot(Vector3 pos, Quaternion rot)
	{

		for(int i=0; i<bullets.Count; i++)
		{
			if(!bullets[i].activeInHierarchy)
			{
				bullets[i].SetActive(true);
				bullets[i].transform.position = GameObject.FindGameObjectWithTag("BulletPoint").transform.position;
				NetworkServer.Spawn(bullets[i]);

				break;
			}
		}
		
	}


	void HandleRotation()
	{
		mousePos = gameObject.GetComponentInChildren<Camera>().ScreenToWorldPoint(Input.mousePosition);
//		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		direction = (mousePos - (Vector2)trans.position).normalized;
		
		angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + (int)initialRotation;
		trans.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	void HandleControls()
	{

		if(Input.GetAxis("Horizontal")<0)
		{
			rb.AddForce(Vector2.left*15);
		}
		
		if(Input.GetAxis("Horizontal")>0)
		{
			rb.AddForce(Vector2.right*15);
		}
		
		if(Input.GetButtonDown("Jump"))
		{
			
			if(jumpCount<2)
			{
				Jump();
			}
		}
		
		if(Input.GetMouseButtonDown(0))
		{
			CmdShoot(transform.position, transform.rotation);
		}
	}

	void OnCollisionEnter2D(Collision2D collider)
	{
		if(collider.gameObject.tag=="Ground")
		{
			jumpCount=0;
		}

		if(collider.gameObject.tag=="Bullet")
		{
			gameObject.SetActive(false);
		}
	}


	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		if (stream.isWriting)
		{
			syncPosition = rb.position;
			stream.Serialize(ref syncPosition);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			rb.position = syncPosition;
		}
	}

	private void SyncedMovement()
	{
		syncTime += Time.deltaTime;
		
		GetComponent<Rigidbody2D>().position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
	}
}
