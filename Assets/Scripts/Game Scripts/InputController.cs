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
	public bool keyPressSpace;
	//mouse
	public bool mouseRButton;
	
	void Update () {

		switch (state){
			case (int)st.inGame:

				keyHoldD = Input.GetKey(KeyCode.D) ? true : false;
				keyHoldA = Input.GetKey(KeyCode.A) ? true : false;
				keyPressSpace = Input.GetKeyDown(KeyCode.Space) ? true : false;
				mouseRButton = Input.GetMouseButton(1) ? true : false;

			break;

		}

		
	}
}
