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
	//private BoxCollider2D boxColl; [not used yet]
	private HealthComponent hp;
	private Animator legsAnimator;
	//private Animator torsoAnimator; [not used yet]
	private WeaponComponent wpn;

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
	public float shotLength = 10;
	public Material bulletMaterial;
	public GameObject emptyBuild;
	private GameObject toCreate;
	public bool justShot = false; [HideInInspector]

	public int equipped = 2;

	//Private
	//>State
	private bool isPlayerDead = false;
	//>Movement
	private bool facingRight = true;
	public bool isAiming = false; [HideInInspector]
	private int jumpTimes;
	private float spd;
	//>Shooting
	//private float shootingRayMax = 50f;
	private Vector3 offsetPosition;

	//Other
	private string toDisplay;
	public float lengthOffsetX = 0.5f, lengthOffsetY = 0.6f;
	public float heightOffsetY;
	public bool isBuilding = false;

	//Testing
	public Image innerHealthBar;
	private int inmune = 0;

	public int valueCollected = 0;

	enum st {
		none = 0,
		normal,
		build,
		dead
	}
	private int state = (int)st.normal;

	void Start () {
		//Get components
		rb = GetComponent<Rigidbody2D>();
		//boxColl = GetComponent<BoxCollider2D>(); [not used yet]
		inputer = inputController.GetComponent<InputController>();
		hp = GetComponent<HealthComponent>();
		legsAnimator = playerLegs.GetComponent<Animator>();
		//torsoAnimator= playerTorso.GetComponent<Animator>(); [not used yet]
		wpn = GetComponent<WeaponComponent>();
		//
		rb.freezeRotation = true;

		
	}
	
	void Update () {

		//TODO: remove, this is just testing. Implement better
		var theBarRectTransform = innerHealthBar.transform as RectTransform;
		//var newWidth = Remap(theBarRectTransform.sizeDelta.x,0,100,0,hp.HealthCurrent );
		float newWidth = map(100,180,hp.HealthCurrent);
		if (newWidth >= 0 && newWidth <= 180)
		theBarRectTransform.sizeDelta = new Vector2(newWidth, theBarRectTransform.sizeDelta.y);

		inmune--;

		if (inmune > 0){
			playerTorso.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f, 0.5f);
			playerLegs.GetComponent<SpriteRenderer>().color =  new Color(1f,1f,1f, 0.5f);
		} else {
			playerTorso.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f, 1f);
			playerLegs.GetComponent<SpriteRenderer>().color =  new Color(1f,1f,1f, 1f);
		}

		switch (state){
			case (int)st.normal: 

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
					if (inputer.mouseLButton) wpn.Shoot(aimDirection); //Gun is single fire.
					else if (inputer.mouseLButtonRel) justShot = false;

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
				toDisplay = ("Value Collected: " + valueCollected );
				GameObject.Find("Text").GetComponent<Text>().text = toDisplay;	

				if (inputer.keyPressQ){
					state = (int)st.build;
					isBuilding = true;
				}
				break;



			case (int)st.none: 
				state = (int)st.normal; 
				break;

			case (int)st.build: 

				Vector2 mousePos = OffsetCalculator.GetMousePos();

				//Building
				if (GameObject.Find("ToBuildGhost(Clone)") == null){
					toCreate = Instantiate(emptyBuild, mousePos, Quaternion.identity );
				}

				if (inputer.mouseLButtonPress) toCreate.GetComponent<DefenseGhost>().Create();

				//Movement & Jumping
				if (jumpTimes <= 0){
					if (inputer.keyHoldD){
						rb.velocity = new Vector2( moveSpeed, rb.velocity.y);
						facingRight = true;	
						
					} else if (inputer.keyHoldA){
						rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
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

				playerLegs.transform.position = playerTorso.transform.position + new Vector3(0,0,1);

				//temporal
				playerTorso.GetComponent<SpriteRenderer>().sprite = sprNormalKira;

				//Reset the Torso sprite to have no rotation
				if (!isPlayerDead) RotateTorsoReset();
				legsAnimator.speed = 1f;

				//Face correct direction
				playerTorso.GetComponent<SpriteRenderer>().flipX = !facingRight;
				playerLegs.GetComponent<SpriteRenderer>().flipX = !facingRight;

				if (inputer.keyPressQ){
					isBuilding = false;
					DestroyImmediate(toCreate);
				}

				if (!isBuilding){ //exit state
					state = (int)st.normal;
				}

				break; 
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
	
	public void Damaged(float val){
		if(inmune <= 0){
			rb.velocity = new Vector2(rb.velocity.x, jumpForce*2);
			inmune = 120;
		
			if (hp.HealthCurrent > 0){
				hp.HealthCurrent -= val;
			}
			if (hp.HealthCurrent <= 0){
				GetKiled();
			}
		}
		
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

	float map(float from, float to, float val){
		return (to*val)/from;
	}
}