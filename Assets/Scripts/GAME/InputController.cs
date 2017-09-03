using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

	enum st{
		none    = 0,
		inGame  = 1,
		inMovie = 2
	}

	private int state = (int)st.inGame;

	//hold
	public bool keyHoldD, keyHoldA;
	//press
	public bool keyPressSpace, keyPressQ, keyPressR;
	//mouse
	public bool mouseRButton, mouseLButton;
	public bool mouseRButtonPress, mouseLButtonPress;
	public bool mouseRButtonRel, mouseLButtonRel;
	
	void Update () {

		switch (state){
			case (int)st.inGame:

				keyHoldD = Input.GetKey(KeyCode.D) ? true : false;
				keyHoldA = Input.GetKey(KeyCode.A) ? true : false;

				keyPressSpace = Input.GetKeyDown(KeyCode.Space) ? true : false;
				keyPressQ = Input.GetKeyDown(KeyCode.Q) ? true : false;
				keyPressR = Input.GetKeyDown(KeyCode.R) ? true : false;
				
				mouseRButton = Input.GetMouseButton(1) ? true : false;
				mouseLButton = Input.GetMouseButton(0) ? true : false;

				mouseRButtonPress = Input.GetMouseButtonDown(1) ? true : false;
				mouseLButtonPress = Input.GetMouseButtonDown(0) ? true : false;

				mouseRButtonRel = Input.GetMouseButtonUp(1) ? true : false;
				mouseLButtonRel = Input.GetMouseButtonUp(0) ? true : false;

			break;

		}

		
	}
}
