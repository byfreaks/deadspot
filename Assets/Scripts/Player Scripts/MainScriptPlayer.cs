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
	public float shotLength = 10;
	public Material bulletMaterial;

	//Private
	//>State
	private bool isPlayerDead = false;
	//>Movement
	private bool facingRight = true;
	public bool isAiming = false; [HideInInspector]
	private int jumpTimes;
	private float spd = 0;
	//>Shooting
	private float shootingRayMax = 50f;
	private Vector3 offsetPosition;

	//Other
	private string toDisplay;
	public float lengthOffsetX = 0.5f, lengthOffsetY = 0.6f;
	public float heightOffsetY;

	//Testing
	public Image innerHealthBar;
	private int inmune = 0;

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

		//TESTING NOTE:
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
			//toDisplay = ("fps " + Mathf.Floor(1.0f / Time.deltaTime) );
			//GameObject.Find("Text").GetComponent<Text>().text = toDisplay;	

			//if (inputer.keyPressQ) GetKiled(); TODO: 
			if (inputer.keyPressQ) Damaged(1);  
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
		//TODO: Clamp shotLength to the distance between offsetPosition and hit.point
		Vector3 lineOffset = OffsetCalculator.GetOffsetPosition(mousePos.y > pos.y, pos, ang, shotLength, shotLength);
		DrawLine(offsetPosition, lineOffset ,Color.white, 0.1f);

		// TODO: effect
		if (hit.collider != null){
			
			//Debug Actual hit
			Debug.DrawRay(offsetPosition,hit.point - (Vector2)offsetPosition,Color.red,0);

			Rigidbody2D hitRb = hit.collider.GetComponent<Rigidbody2D>();
			if (hitRb.bodyType != RigidbodyType2D.Static){
				hitRb.velocity = new Vector2(30,0); //temporal kickback targert	
				hit.collider.GetComponent<HealthComponent>().Damage(50); //placeholder damager
			}

			

			
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
		//lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
		lr.material = bulletMaterial;
		//lr.SetColors(color, color);
		lr.startColor = color;
		lr.endColor = color;
		//lr.SetWidth(0.1f, 1.2f);
		lr.startWidth = 0.1f;
		lr.endWidth = 1.2f;
		lr.SetPosition(0, start);
		lr.SetPosition(1, end);
		GameObject.Destroy(myLine, duration);
	}
	
	public void Damaged(float val){
		if(inmune <= 0){
			rb.velocity = new Vector2(rb.velocity.x, jumpForce*2);
			inmune = 120;
		
			if (hp.HealthCurrent <= 0){
				GetKiled();
			} else {
				hp.HealthCurrent -= val;
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

	void OnDrawGizmos(){
		Gizmos.DrawSphere(offsetPosition, 2);
	}

	//TODO: Erase, or at leat figure out
	float Remap (float value, float from1, float to1, float from2, float to2) {
    	return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}

	float map(float from, float to, float val){
		return (to*val)/from;
	}
}
