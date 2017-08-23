using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScriptPlayer : MonoBehaviour {

	//Sprites
	public Sprite sprAimingKira;
	public Sprite sprNormalKira;

	//Components
	private Rigidbody2D rb;
	private BoxCollider2D boxColl;
	private HealthComponent hp;

	//Body parts
	[Header("Body parts")]
	public GameObject playerTorso;
	public GameObject playerLegs;

	[Header("Game Scripts")]
	public GameObject inputController;
	public InputController inputer;

	//Design Public
	[Header("Physics settings")]
	public float moveSpeed = 3;
	public float moveAim = 100;
	public float jumpForce = 3;

	[Header("Logic")]
	public LayerMask shootingLayer;

	//Other
	private string toDisplay;
	public float wOffsetX = 0f, wOffsetY = 0f;
	private Vector3 mousePos;

	//Private
	private bool facingRight = true;
	private bool isAiming = false;
	private int jumpTimes;

	private float spd = 0;
	private float shootingRayMax = 50f;

	void Start () {
		//Get components
		rb = GetComponent<Rigidbody2D>();
		boxColl = GetComponent<BoxCollider2D>();
		inputer = inputController.GetComponent<InputController>();
		hp = GetComponent<HealthComponent>();
		
		//
		rb.freezeRotation = true;
	}
	
	void Update () {

		//Aim
		if (inputer.mouseRButton){
			isAiming = true;
		} else {			
			isAiming = false;
		}

		//Movement
		if (isAiming) spd = moveSpeed - moveAim;
		else spd = moveSpeed;

		
		if (inputer.keyHoldD){
			rb.velocity = new Vector2( spd, rb.velocity.y);
			facingRight = true;
		} else if (inputer.keyHoldA){
			rb.velocity = new Vector2(-spd, rb.velocity.y);
			facingRight = false;
		}
		if (inputer.keyPressSpace){
			rb.velocity = new Vector2(rb.velocity.x, jumpForce );
			jumpTimes++;
		}

		
		
		//Animation
		if (isAiming){
			playerTorso.GetComponent<SpriteRenderer>().sprite = sprAimingKira;

			//TODO declare proper vars
			mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        	playerTorso.transform.rotation = Quaternion.LookRotation(Vector3.forward, (mousePos - transform.position) );
			playerTorso.transform.position = new Vector3(playerTorso.transform.position.x, playerTorso.transform.position.y, playerLegs.transform.position.z-0.1f);

			if (mousePos.x > transform.position.x) facingRight = true;
			else facingRight = false;

			if (inputer.mouseLButton){
				Shoot();
			}

		} else {
			playerTorso.GetComponent<SpriteRenderer>().sprite = sprNormalKira;
			playerTorso.transform.localEulerAngles = new Vector3(0, 0, 0);

		}

		playerTorso.GetComponent<SpriteRenderer>().flipX = !facingRight;
		playerLegs.GetComponent<SpriteRenderer>().flipX = !facingRight;

		//Debug FPS
		toDisplay = ("fps " + Mathf.Floor(1.0f / Time.deltaTime) );
		GameObject.Find("Text").GetComponent<Text>().text = toDisplay;	

		//Testing
		if (inputer.keyPressQ) hp.Damage(10f);
		
	}

	void Shoot(){
		RaycastHit2D hit;
		Vector3 weaponOffset;
		
		weaponOffset = new Vector3(wOffsetX, wOffsetY, 0);

		hit = Physics2D.Raycast( (transform.position+weaponOffset), (new Vector3(0,Random.Range(-20,20),0) + mousePos) - transform.position, 1000, shootingLayer);

		if (hit.collider != null){
			Debug.DrawRay((transform.position+weaponOffset), hit.point - (Vector2)transform.position );
			hit.collider.GetComponent<Rigidbody2D>().velocity = new Vector2(30,0);
			DrawLine(transform.position+weaponOffset, hit.point, Color.white, 0.1f);
		}
		
		
	}

	void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.1f)
	{
		GameObject myLine = new GameObject();
		myLine.transform.position = start;
		myLine.AddComponent<LineRenderer>();
		LineRenderer lr = myLine.GetComponent<LineRenderer>();
		lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
		lr.SetColors(color, color);
		lr.SetWidth(0.1f, 1.2f);
		lr.SetPosition(0, start);
		lr.SetPosition(1, end);
		GameObject.Destroy(myLine, duration);
	}

	
}