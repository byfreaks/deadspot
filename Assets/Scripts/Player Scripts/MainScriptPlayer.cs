using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScriptPlayer : MonoBehaviour {

	//Sprites (temporal)
	public Sprite sprAimingKira;
	public Sprite sprNormalKira;

	//Components
	private Rigidbody2D rb;
	private BoxCollider2D boxColl;
	private HealthComponent hp;
	private Animator legsAnimator;
	private Animator torsoAnimator;

	//Body parts
	[Header("Body parts")]
	public GameObject playerTorso;
	public GameObject playerLegs;

	//Controllers
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

	//Private
	//>State
	private bool isPlayerDead = false;
	//>Movement
	private bool facingRight = true;
	private bool isAiming = false;
	private int jumpTimes;
	private float spd = 0;
	//>Shooting
	private float shootingRayMax = 50f;
	private Vector3 offsetPosition;

	//Other
	private string toDisplay;
	public float lengthOffsetX = 0.5f, lengthOffsetY = 0.6f;
	public float heightOffsetY;

	enum st {
		none = 0,
		normal,
		dead
	}
	private int state = (int)st.normal;

	void Start () {
		//Get components
		rb = GetComponent<Rigidbody2D>();
		boxColl = GetComponent<BoxCollider2D>();
		inputer = inputController.GetComponent<InputController>();
		hp = GetComponent<HealthComponent>();
		legsAnimator = playerLegs.GetComponent<Animator>();
		torsoAnimator= playerTorso.GetComponent<Animator>();
		

		//
		rb.freezeRotation = true;
	}
	
	void Update () {
		switch (state){
			case (int)st.normal:                                               ///NORMAL STATE NOTE:

				//Aim
			if (inputer.mouseRButton){
				isAiming = true;
			} else {			
				isAiming = false;
			}

			//Movement
			if (isAiming) spd = moveSpeed - moveAim;
			else spd = moveSpeed;

			if (jumpTimes <= 0){
				if (inputer.keyHoldD){
					rb.velocity = new Vector2( spd, rb.velocity.y);
					facingRight = true;	
					
				} else if (inputer.keyHoldA){
					rb.velocity = new Vector2(-spd, rb.velocity.y);
					facingRight = false;

				}
				
				if (inputer.keyPressSpace){
					rb.velocity = new Vector2(rb.velocity.x, jumpForce );
					//jumpTimes++;
				}
			}

			//Animation
			if (Mathf.Abs(rb.velocity.x) != 0) legsAnimator.SetBool("aWalking", true);
			else legsAnimator.SetBool("aWalking", false);

			if (isAiming){
				//temporal
				playerTorso.GetComponent<SpriteRenderer>().sprite = sprAimingKira;

				Vector3 aimDirection;
				RotateTorso(out aimDirection);
				if (inputer.mouseLButtonPress) Shoot(aimDirection); //Gun is single fire.

				legsAnimator.speed = 0.5f;
				
			} else {
				//temporal
				playerTorso.GetComponent<SpriteRenderer>().sprite = sprNormalKira;

				//Reset the Torso sprite to have no rotation
				if (!isPlayerDead) RotateTorsoReset();

				legsAnimator.speed = 1f;

			}

			//Face correct direction
			playerTorso.GetComponent<SpriteRenderer>().flipX = !facingRight;
			playerLegs.GetComponent<SpriteRenderer>().flipX = !facingRight;

			//Debug FPS
			toDisplay = ("fps " + Mathf.Floor(1.0f / Time.deltaTime) );
			GameObject.Find("Text").GetComponent<Text>().text = toDisplay;	

			if (inputer.keyPressQ) GetKiled();
				break;



			case (int)st.none:                                                      // NONE STATE NOTE:
			state = (int)st.normal; 
				break;
		}

		
		
	}

	void Shoot(Vector3 dir, float defaultMargin = 0, float maxDistance = 500f){
		
		//Mouse and playerTorso position variables.
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
		Vector3 pos = playerTorso.transform.position + new Vector3(0,heightOffsetY,0);
		
		//Ray Propierties
		RaycastHit2D hit;
		float rayMargin = Random.Range(-defaultMargin,defaultMargin);
		Vector3 rayDirection = (new Vector3(0, rayMargin, 0) + dir);
		float rayDistance = maxDistance;

		//Calculate Offset
		float ang = Vector2.Angle(Vector2.right,dir); 
		offsetPosition = OffsetCalculator.GetOffsetPosition(mousePos.y > pos.y, pos, ang, lengthOffsetX, lengthOffsetY);

		//Shoot
		hit = Physics2D.Raycast( pos, rayDirection, rayDistance, shootingLayer);

		//Cosmetic

		//DrawLine(gunOffset.transform.position,rayDirection+pos,Color.white, 0.1f);
		// FIXME: when hit.point is null, the ray line gets drawn to the origin of the scene,
		// where it should extend to the shot direction a defined distance. 
		DrawLine(offsetPosition,hit.point,Color.white, 0.1f);

		// TODO: effect
		if (hit.collider != null){

			Rigidbody2D hitRb = hit.collider.GetComponent<Rigidbody2D>();

			if (hitRb.bodyType != RigidbodyType2D.Static) hitRb.velocity = new Vector2(30,0); //temporal kickback targert	

			
		}
		
	}

	void RotateTorso(out Vector3 aimDirection){
		//Get mouse and torso position vector3
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
		Vector3 torsoPos = playerTorso.transform.position;

		//Aim torso towards Mouse
        playerTorso.transform.rotation = Quaternion.LookRotation(Vector3.forward, (mousePos - transform.position) );

		//Place Torso above Legs by 0.1
		float diff = 0.1f;
		torsoPos = new Vector3(torsoPos.x, torsoPos.y, playerLegs.transform.position.z - diff);

		//Face player towards aiming direction
		if (mousePos.x > transform.position.x) facingRight = true;
		else facingRight = false;

		aimDirection = mousePos - transform.position;
	}

	void RotateTorsoReset(){
		playerTorso.transform.localEulerAngles = new Vector3(0, 0, 0);
	}

	void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.1f){
		//FIXME: Update obsolete methods and customize look.
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
	

	void GetKiled(){
		isPlayerDead = true;
		
		playerTorso.AddComponent<Rigidbody2D>();
		playerLegs.AddComponent<Rigidbody2D>();

		playerTorso.GetComponent<Rigidbody2D>().AddTorque(300);
		playerLegs.GetComponent<Rigidbody2D>().AddTorque(300);

		playerTorso.GetComponent<Rigidbody2D>().velocity = new Vector2( Random.Range(-400,400), Random.Range(200,400) );
		playerLegs.GetComponent<Rigidbody2D>().velocity = new Vector2( Random.Range(-400,400), Random.Range(200,400) );
	}

	void OnDrawGizmos(){
		Gizmos.DrawSphere(offsetPosition, 2);
	}
}
