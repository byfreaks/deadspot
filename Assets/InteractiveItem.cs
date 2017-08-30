using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveItem : MonoBehaviour {

	private bool canBeUsed;
	private bool open = false;
	private string tagName = "something";

	public InputController input;

	void Start(){
		input = new InputController();
	
		tagName = "woodbench";

	}
	
	void OnTriggerStay2D(){
		if (input.keyPressQ && canBeUsed && !open){
			open = true;
		}
	}

	

	void Updte(){
		if (open){
			
		}
	}

}
