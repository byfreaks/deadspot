using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjective : MonoBehaviour {

	private GameObject objToAttack;
	public bool collision;
	// Use this for initialization
	void Start () {
		 objToAttack = transform.parent.GetComponent<MainScriptAI>().objective;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log("SSSSS");
		if(other == objToAttack.GetComponent<Collider2D>()){
			collision = true;
			Debug.Log("Colisionando");
		}else{
			collision = false;
		}
		
	}
}
