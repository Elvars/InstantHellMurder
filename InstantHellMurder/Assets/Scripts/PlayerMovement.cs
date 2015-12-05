using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerMovement : NetworkBehaviour 
{


	float speed = 10;
	
	float thrusting;

	int moveForce;

	bool m_focus = true;
	

	public PlayerCombat tc;
	
	
	float turrentSendTimer = 0.0f;
	float turrentSendDelay = 0.1f;

	//uudet jutut


	[SyncVar]
	public float turretAngle;

	private Vector2 direction;
	private Vector2 mousePos;

	private Transform weaponHinge;
	private Transform bulletPoint;

	public enum WeaponRotation
	{
		Up = -90,
		Right = 0,
		Down = 90,
		Left = 180
	}
	
	public WeaponRotation initialRotation;
	Rigidbody2D rb;

	[SyncVar]
	int jumpCount;


	void Awake()
	{
		weaponHinge = gameObject.transform.GetChild(0);
		rb = GetComponent<Rigidbody2D>();

		tc = gameObject.GetComponent<PlayerCombat>();
	}

	
	void Update() 
	{

		if (NetworkClient.active)
			UpdateClient();

	}

	
	void OnApplicationFocus(bool value)
	{
		m_focus = value;
	}
	
	void UpdateClient()
	{

		if (!isLocalPlayer)
		{
			Vector3 trotationVector = new Vector3 (0, 0, turretAngle);
			weaponHinge.transform.rotation = Quaternion.Euler(trotationVector);
			return;
		}
		
		if (!m_focus)
			return;
		

		HandlePlayerMovement();
		
		if (Input.GetMouseButtonDown(0))
		{
			tc.CmdFireTurret();
		}

		
		if (Input.GetKey(KeyCode.F1))
		{
			//tc.CmdKillSelf();
		}

		Vector3 cpos = transform.position;
		cpos.z = Camera.main.transform.position.z;
		Camera.main.transform.position = cpos;


		Vector3 mouse_pos = Input.mousePosition;
		mouse_pos.z = 0.0f; 
		Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
		mouse_pos.x = mouse_pos.x - object_pos.x;
		mouse_pos.y = mouse_pos.y - object_pos.y;
		float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg + (int)initialRotation;
		Vector3 rotationVector = new Vector3 (0, 0, angle);
		weaponHinge.transform.rotation = Quaternion.Euler(rotationVector);
		CmdRotateTurret(angle);


	}


	void HandlePlayerMovement()
	{
		if(Input.GetAxis("Horizontal")<0)
		{
			CmdAddForce(0);
		}
		
		if(Input.GetAxis("Horizontal")>0)
		{
			CmdAddForce(1);
		}
		
		if(Input.GetButtonDown("Jump"))
		{
			
			if(jumpCount<2)
			{
				CmdJump();
			}
		}

	}

	[Client]
	public void CmdAddForce(int dir)
	{

		if(dir==0)
		{
			rb.AddForce(Vector2.left*15);
		}
		if(dir==1)
		{
			rb.AddForce(Vector2.right*15);
		}
	}


	[Command]
	public void CmdRotateTurret(float angle)
	{
//		if (!tc.alive)
//			return;
//		
//		if (PlayGame.GetComplete())
//			return;
//

	
		Vector3 rotationVector = new Vector3 (0, 0, angle);
		weaponHinge.transform.rotation = Quaternion.Euler(rotationVector);
		turretAngle = angle;	
	}
	

	
	[Client]
	void CmdJump()
	{
		jumpCount++;
		rb.AddForce(Vector2.up*500);
		
	}


	void OnCollisionEnter2D(Collision2D collider)
	{
		if(collider.gameObject.tag=="Ground")
		{
			jumpCount=0;
		}
		
	
	}

}
