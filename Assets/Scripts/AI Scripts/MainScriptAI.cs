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

	//Attributes
	[Header("Physics settings")]
	public float movespeed;
	public float attackDistance;
	private int attackTime = 100; 
	
	//States
	private bool followingObj = false;
	private bool isAttacking = false;
	private bool facingRight = true;
	private bool inAttack = false;

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
		attackDistance = Random.Range(17f,21f);
		contAttack = 0;
	}
	
	// Update is called once per frame
	void Update () {

		//STATE: dead
		if(hthCom.HealthCurrent == 0){
			
			//FALTA Animacion de Morir
			Destroy(this.gameObject);

		//STATE: in Attack
		}else if(inAttack){
			if(contAttack == 1){
				
				if(attackCollider.GetComponent<AttackObjective>().collision){
					Debug.Log("Ataco Aqui");
					objective.GetComponent<MainScriptPlayer>().Damaged(20);
				}
				contAttack++;

			}else if(contAttack>=attackTime){
				
				Debug.Log("Termino el ataque");
				contAttack=0;
				Destroy(attackCollider);
				inAttack = false;
			
			}else{
				contAttack++;
				Debug.Log(contAttack);
			}

			if (facingRight && attackCollider != null){
				attackCollider.GetComponent<BoxCollider2D>().offset = new Vector2(attackPosition,0);
			} else {
				attackCollider.GetComponent<BoxCollider2D>().offset = new Vector2(-attackPosition,0);
			}

		}else{

			//Set State
			followingObj = (objective!=null);

			//STATE: idle
			if(!followingObj){
				//Do something

			//STATE: following objective
			}else{

				//Detect position objective
				this.facingRight = detectPositionObj();
				sprRr.flipX = !this.facingRight;

				//Move to objective
				if(!this.isAttacking) moveTo(this.facingRight);

				//Detect collision with the objective
				this.isAttacking = detectCollision(this.objective);

				//Attack
				if(this.isAttacking){
					inAttack = true;
					
					attackCollider = Instantiate(refAttackCollider, this.transform.position, Quaternion.identity);
					attackCollider.transform.parent = this.transform;

					//Empieza Animacion de atacar
					Debug.Log("Empieza el ataque");
					
				}
				
			}
		}
		
	}

	bool detectPositionObj(){
		if(objective.transform.position.x < this.transform.position.x){
			//Left
			return false;
		}else{
			//Right
			return true;
		}
	}

	bool detectCollision(GameObject a){
		if(this.facingRight && Physics2D.Raycast(this.transform.position, Vector2.right, attackDistance, objectiveLayer).collider==a.GetComponent<BoxCollider2D>()){
			return true; //Collision right
		}else if(!this.facingRight && Physics2D.Raycast(this.transform.position, Vector2.left, attackDistance, objectiveLayer).collider==a.GetComponent<BoxCollider2D>()){
			return true; //Collision left
		}
		return false; //NO collision
	}

	void moveTo(bool a){
		//True == Right/False == Left
		rb.velocity = !a ? new Vector2(-movespeed, rb.velocity.y) : rb.velocity = new Vector2(movespeed, rb.velocity.y);
		
	}


	

}
