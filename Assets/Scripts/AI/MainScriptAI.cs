using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScriptAI : MonoBehaviour {

	//Attack Collider
	public GameObject refAttackCollider;
	private GameObject attackCollider;
	private int contAttack;
	float attackPosition = 15f;
	
	//Objective to attack
	public GameObject objective;
	public LayerMask objectiveLayer;

	//Second objective to attack
	public GameObject objSecond;
	public LayerMask objSecondLayer;

	//Attributes
	[Header("Physics settings")]
	public float movespeed;
	public float attackDistance;
	private int attackTime = 50; 
	
	//States
	private bool followingObj = false; 
	private bool isAttacking = false;
	private bool facingRight = true;
	private bool inAttack = false;
	private bool wait = false;

	//Components
	private Rigidbody2D rb;
	private BoxCollider2D bColl;
	private Animator anm;
	private SpriteRenderer sprRr;
	private HealthComponent hthCom;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		bColl = GetComponent<BoxCollider2D>();
		anm = GetComponent<Animator>();
		sprRr = GetComponent<SpriteRenderer>();
		hthCom = GetComponent<HealthComponent>();

		rb.freezeRotation = true;

		movespeed = Random.Range(80f,130f);
		attackDistance = Random.Range(17f,27f);
		contAttack = 0;
	}
	
	// Update is called once per frame
	void Update () {

		//STATE: dead
		if(hthCom.HealthCurrent == 0){
			
			kill();

		//STATE: in Attack
		}else if(inAttack){
			//Moment of attack (Frame after of collision)
			if(contAttack == 1){
				//Check if hit the objective
				if(attackCollider.GetComponent<AttackObjective>().collision){
					objective.GetComponent<MainScriptPlayer>().Damaged(20);
				
				}else if(this.objSecond!=null){ //Check if hit the second objective
					if(attackCollider.GetComponent<AttackObjective>().collisionSObj){
						objSecond.GetComponent<HealthComponent>().Damage(25);
					}
				}
				contAttack++;
			
			//End attack animation 
			}else if(contAttack>=attackTime){
				
				contAttack=0;
				Destroy(attackCollider);
				inAttack = false;
			
			//Doing attack animation
			}else{
				contAttack++;
			}

			//Set attack hit box to the left/right
			if (facingRight && attackCollider != null){
				attackCollider.GetComponent<BoxCollider2D>().offset = new Vector2(attackPosition,0);
			} else if (!facingRight && attackCollider != null) {
				attackCollider.GetComponent<BoxCollider2D>().offset = new Vector2(-attackPosition,0);
			}
			

		//OTHER STATES
		}else{

			//Set State
			followingObj = (objective!=null);

			//STATE: idle
			if(!followingObj){
				//Do something

			//STATE: following objective
			}else{

				short pos = detectPositionObj();
				//Detect position objective
				if(pos == -1){
					
					//Left
					wait = false;
					facingRight = false;
					sprRr.flipX = !this.facingRight;

				}else if(pos == 1){
					
					//Right
					wait = false;
					facingRight = true;
					sprRr.flipX = !this.facingRight;

				}else if(pos == 0){
					
					//Top/down
					wait = true;
				}

				

				//Move to objective
				if(!this.isAttacking && !wait) moveTo(this.facingRight);

				//Set second objective
				setSecondObjective();

				//Detect collision with the objective
				this.isAttacking = detectCollision(this.objective, this.objectiveLayer);
				//Detec collision with the second objective
				if(this.objSecond!=null)this.isAttacking = detectCollision(this.objSecond, this.objSecondLayer);

				//Attack
				if(this.isAttacking){
					inAttack = true;
					
					attackCollider = Instantiate(refAttackCollider, this.transform.position, Quaternion.identity);
					attackCollider.transform.parent = this.transform;
					attackCollider.layer = 0;

					//Empieza Animacion de atacar
					
				}
				
			}
		}
		
	}

	short detectPositionObj(){
		if(objective.transform.position.x  < this.transform.position.x - attackDistance){
			//Left
			return -1;
		}else if(objective.transform.position.x > this.transform.position.x  + attackDistance){
			//Right
			return 1;
		}else{
			return 0;
		}
	}

	bool detectCollision(GameObject a, LayerMask b){
		if(this.facingRight && Physics2D.Raycast(this.transform.position, Vector2.right, attackDistance, b).collider==a.GetComponent<BoxCollider2D>()){
			return true; //Collision right
		}else if(!this.facingRight && Physics2D.Raycast(this.transform.position, Vector2.left, attackDistance, b).collider==a.GetComponent<BoxCollider2D>()){
			return true; //Collision left
		}
		return false; //NO collision
	}

	void moveTo(bool a){
		//True == Right/False == Left
		rb.velocity = !a ? new Vector2(-movespeed, rb.velocity.y) : rb.velocity = new Vector2(movespeed, rb.velocity.y);
		
	}

	void kill(){
		//FALTA Animacion muerte
		Destroy(this.gameObject);
	}
	
	void setSecondObjective(){
		if(this.facingRight && Physics2D.Raycast(this.transform.position, Vector2.right, attackDistance, objSecondLayer).collider!=null){
			objSecond = Physics2D.Raycast(this.transform.position, Vector2.right, attackDistance, objSecondLayer).collider.gameObject;//Collision right
		}else if(!this.facingRight && Physics2D.Raycast(this.transform.position, Vector2.left, attackDistance, objSecondLayer).collider!=null){
			objSecond = Physics2D.Raycast(this.transform.position, Vector2.left, attackDistance, objSecondLayer).collider.gameObject;//Collision left
		}

	}
}
