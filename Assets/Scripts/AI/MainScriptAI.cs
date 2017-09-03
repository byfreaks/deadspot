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

	//Pathfinding
	private GameObject objZone;
	public GameObject refPathController;
	private PathFinding pathFinding;
	public int currentZone;

	//Attributes
	[Header("Physics settings")]
	public float movespeed;
	public float climbspeed;
	public float attackDistance;
	public float damageOne;
	public float damageTwo;
	private int attackTime = 50; 
	private int contClimb = 70;
	
	//States
	[Header("STATES")]
	public bool followingObj = false; 
	private bool isAttackingOne = false;
	private bool isAttackingTwo = false;
	private bool facingRight = true;
	public bool inAttack = false;
	public bool wait = false;
	public bool climbUp = false;
	public bool climbDown = false;
	public bool climbing = false;

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
		pathFinding = refPathController.GetComponent<PathFinding>();

		rb.freezeRotation = true;

		this.movespeed = Random.Range(80f,130f);
		this.climbspeed = 80;
		this.attackDistance = Random.Range(17f,27f);
		this.contAttack = 0;
		this.damageOne = 25;
		this.damageTwo = 20;
	}
	
	// Update is called once per frame
	void Update () {

		//STATE: dead
		if(hthCom.HealthCurrent == 0){
			
			this.kill();

		//STATE: in Attack
		}else if(this.inAttack){
			//Moment of attack (Frame after of collision)
			if(this.contAttack == 1){
				if(this.isAttackingOne){
					//Check if hit the objective
					if(attackCollider.GetComponent<AttackObjective>().collision){
						objective.GetComponent<MainScriptPlayer>().Damaged(damageOne);					
					}
				}else if(this.isAttackingTwo){
					//Check if hit the second objective
					if(attackCollider.GetComponent<AttackObjective>().collisionSObj){
						objSecond.GetComponent<DefenseBehaviour>().Damage(damageTwo);
					}
				}
				this.contAttack++;
			
			//End attack animation 
			}else if(this.contAttack>=this.attackTime){
				
				this.contAttack=0;
				Destroy(attackCollider);
				this.inAttack = false;
			
			//Doing attack animation
			}else{
				this.contAttack++;
			}

			//Set attack hit box to the left/right
			if (this.facingRight && attackCollider != null){
				attackCollider.GetComponent<BoxCollider2D>().offset = new Vector2(attackPosition,0);
			} else if (!this.facingRight && attackCollider != null) {
				attackCollider.GetComponent<BoxCollider2D>().offset = new Vector2(-attackPosition,0);
			}
			

		//Climbing
		}else if(this.climbing){
			if(this.contClimb>0){
				if(this.climbUp){
					crossZone(1);
				}else if(this.climbDown){
					crossZone(-1);
				}
				this.contClimb--;
			}else{
				this.climbing = false;
				this.climbUp = false;
				this.climbDown = false;
				this.contClimb = 60;
			}

		//OTHER STATES
		}else{

			//Set State
			this.followingObj = (objective!=null);

			//STATE: idle
			if(!this.followingObj){
				//Do something

			//STATE: following objective
			}else{

				//Check player zone
				if(pathFinding.playerZone!=this.currentZone){
					objZone = pathFinding.findConnection(this.currentZone);
					
					short pos = detectPositionObj(objZone, 10);
					//Detect position objective
					if(pos == -1){

						//Left
						this.wait = false;
						this.facingRight = false;
						sprRr.flipX = !this.facingRight;

					}else if(pos == 1){
						
						//Right
						this.wait = false;
						this.facingRight = true;
						sprRr.flipX = !this.facingRight;

					}else if(pos == 0){
						
						//Top/down
						this.wait = true;
					}
					
					//Walking to connection
					if(!this.wait) moveTo(this.facingRight);
					//Cross to another zone
					else{
						climbing = true;
						//UP
						if(pathFinding.playerZone>this.currentZone){
							climbUp = true;
						//Down
						}else if(pathFinding.playerZone<this.currentZone){
							climbDown = true;
						}
					} 

				}else{

					//RESTART
					this.isAttackingOne = false;
					this.isAttackingTwo = false;

					short pos = detectPositionObj(objective, this.attackDistance);
					//Detect position objective
					if(pos == -1){
						
						//Left
						this.wait = false;
						this.facingRight = false;
						sprRr.flipX = !this.facingRight;

					}else if(pos == 1){
						
						//Right
						this.wait = false;
						this.facingRight = true;
						sprRr.flipX = !this.facingRight;

					}else if(pos == 0){
						
						//Top/down
						this.wait = true;
					}

					

					//Move to objective
					if(!this.isAttackingOne && !this.isAttackingTwo && !wait){
						this.moveTo(this.facingRight);
					}

					//Set second objective
					this.setSecondObjective();

					//Detect collision with the objective
					this.isAttackingOne = this.detectCollision(this.objective, this.objectiveLayer);
					//Detec collision with the second objective
					if(this.objSecond!=null)this.isAttackingTwo = this.detectCollision(this.objSecond, this.objSecondLayer);

					//Attack
					if(this.isAttackingOne || this.isAttackingTwo){
						this.inAttack = true;
						
						attackCollider = Instantiate(refAttackCollider, this.transform.position, Quaternion.identity);
						attackCollider.transform.parent = this.transform;
						//Empieza Animacion de atacar
						
					}
				
				}

				
				
			}
		}
		
	}

	short detectPositionObj(GameObject obj, float a){
		if(obj != null){
			if(obj.transform.position.x  < this.transform.position.x - a){
				//Left
				return -1;
			}else if(obj.transform.position.x > this.transform.position.x  + a){
				//Right
				return 1;
			}else{
				return 0;
			}
		}else{
			if(this.facingRight){
				return 1;
			}
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

	void crossZone(short a){
		//UP
		if(a == 1){
			//Start Animation UP
			rb.velocity = new Vector2(0, climbspeed);
		//Down
		}else if(a==-1){
			//Start Animation Down
			rb.velocity = new Vector2(0, -climbspeed);
		}
	}
}
