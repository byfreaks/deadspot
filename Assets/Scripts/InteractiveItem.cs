using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveItem : MonoBehaviour {

	private bool canBeUsed;
	private bool open = false;

	public InputController input;

	void Start(){
		input = new InputController();
		canBeUsed = true;
	}
	
	void OnTriggerStay2D(){
		if (input.keyPressQ && canBeUsed && !open){
			open = true;
		}
	}

	

	void Update(){
		if (open){
			
		}
	}

}
