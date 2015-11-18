using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	Rigidbody2D rb;

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

	void Awake()
	{
		rb = gameObject.GetComponent<Rigidbody2D>();

		bullets = new List<GameObject>();
		for (int i=0; i<20; i++) 
		{
			GameObject obj = (GameObject)Instantiate (bullet);
			obj.SetActive(false);
			bullets.Add(obj);
		}
	}


	void Start()
	{
		trans = gameObject.transform.GetChild(0);

	}
	

	// Update is called once per frame
	void Update () 
	{
		if(Input.GetAxis("Horizontal")<0)
		{
			rb.AddForce(Vector2.left*10);
		}
		
		if(Input.GetAxis("Horizontal")>0)
		{
			rb.AddForce(Vector2.right*10);
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
			Shoot();
		}

		mousePos = gameObject.GetComponentInChildren<Camera>().ScreenToWorldPoint(Input.mousePosition);

		direction = (mousePos - (Vector2)trans.position).normalized;

		angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + (int)initialRotation;
		trans.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

//		if(transform.GetChild(0).transform.rotation.z<360)
//		{
//			transform.GetChild(0).transform.Rotate(90, 0, 0);
//		}


	}

	void Jump()
	{
		jumpCount++;
		rb.AddForce(Vector2.up*500);

	}

	void Shoot()
	{
		for(int i=0; i<bullets.Count; i++)
		{
			if(!bullets[i].activeInHierarchy)
			{
				bullets[i].SetActive(true);
				bullets[i].transform.position = GameObject.FindGameObjectWithTag("BulletPoint").transform.position;
				break;
			}
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
}
