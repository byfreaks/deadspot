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

	//Body parts
	[Header("Body parts")]
	public GameObject playerTorso;
	public GameObject playerLegs;

	//Design Public
	[Header("Game Scripts")]
	public GameObject inputController;
	private InputController inputer;

	[Header("Physics settings")]
	public float moveSpeed = 3;
	public float moveAim = 100;
	public float jumpForce = 3;

	//Other
	private string toDisplay;

	//Private
	private bool facingRight = true;
	private bool isAiming = false;
	private float spd = 0;


	void Start () {
		//Get components
		rb = GetComponent<Rigidbody2D>();
		boxColl = GetComponent<BoxCollider2D>();
		inputer = inputController.GetComponent<InputController>();
		

		rb.freezeRotation = true;
	}
	
	void Update () {

		if (inputer.mouseRButton){
			toDisplay = "State: Aiming";
			isAiming = true;
		} else {
			toDisplay = "State: Normal";
			isAiming = false;
		}

		//Movement
		if (isAiming) spd = moveSpeed - moveAim;
		else spd = moveSpeed;

		if (inputer.keyHoldD){
			rb.velocity = new Vector2( spd, rb.velocity.y);
			facingRight = true;
		}
		if (inputer.keyHoldA){
			rb.velocity = new Vector2(-spd, rb.velocity.y);
			facingRight = false;
		}
		if (inputer.keyPressSpace){
			rb.velocity = new Vector2(rb.velocity.x, jumpForce );
		}


		GameObject.Find("Text").GetComponent<Text>().text = toDisplay;	
		
		//Animation
		if (isAiming){
			playerTorso.GetComponent<SpriteRenderer>().sprite = sprAimingKira;

			//TODO declare proper vars
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        	playerTorso.transform.rotation = Quaternion.LookRotation(Vector3.forward, (mousePos - transform.position) );

			Debug.Log(mousePos - transform.position);

			if (mousePos.x > transform.position.x) facingRight = true;
			else facingRight = false;

		} else {
			playerTorso.GetComponent<SpriteRenderer>().sprite = sprNormalKira;
			playerTorso.transform.localEulerAngles = new Vector3(0, 0, 0);

		}

		playerTorso.GetComponent<SpriteRenderer>().flipX = !facingRight;
		playerLegs.GetComponent<SpriteRenderer>().flipX = !facingRight;
		
	}
}