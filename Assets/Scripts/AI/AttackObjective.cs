using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjective : MonoBehaviour {

	private GameObject objToAttack;
	private GameObject secObj;
	public bool collision;
	public bool collisionSObj;
	// Use this for initialization
	void Start () {
		 objToAttack = transform.parent.GetComponent<MainScriptAI>().objective;
		 secObj = transform.parent.GetComponent<MainScriptAI>().objSecond;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other == objToAttack.GetComponent<Collider2D>()){
			collision = true;
		}else if(secObj!=null){
			if(other == secObj.GetComponent<Collider2D>()){
				collisionSObj = true;
			}
		}else{
			collisionSObj = false;
			collision = false;
		}
		
	}
}
