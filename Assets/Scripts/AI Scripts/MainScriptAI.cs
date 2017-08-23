using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScriptAI : MonoBehaviour {

	//Objective to attack
	public GameObject objective;

	//Attributes
	[Header("Physics settings")]
	public float movespeed = 200;
	
	//States
	private bool isMoving = true;
	private bool isAttacking = false;
	private bool facingRight = true;

	//Components
	private Rigidbody2D rb;
	private BoxCollider2D bColl;
	private Animator anm;
	private SpriteRenderer sprRr;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		bColl = GetComponent<BoxCollider2D>();
		anm = GetComponent<Animator>();
		sprRr = GetComponent<SpriteRenderer>();

		rb.freezeRotation = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(objective!=null){
			//Detect objective
			if(objective.transform.position.x < this.transform.position.x){
				this.facingRight = false;
				sprRr.flipX = true;
			}else{
				this.facingRight = true;
				sprRr.flipX = false;
			}

			//Collision
			if(this.facingRight && Physics2D.Raycast(this.transform.position, Vector2.right, 40).collider==objective.GetComponent<BoxCollider2D>()){
				this.isAttacking = true;
				this.isMoving = false;
				Debug.Log("Is attacking right: "+ this.isAttacking);
			}else if(!this.facingRight && Physics2D.Raycast(this.transform.position, Vector2.left, 40).collider==objective.GetComponent<BoxCollider2D>()){
				this.isAttacking = true;
				this.isMoving = false;
				Debug.Log("Is attacking left: "+ this.isAttacking);
			}
			
			//Attack

			//Move
			if(isMoving){
				rb.velocity = !this.facingRight ? new Vector2(-movespeed, rb.velocity.y) : rb.velocity = new Vector2(movespeed, rb.velocity.y);
			}
			
			//Reset
			this.isMoving = true;
			this.isAttacking = false;
		}
	}
}
