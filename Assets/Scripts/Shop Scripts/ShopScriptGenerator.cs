using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScriptGenerator : MonoBehaviour {

	public bool infinitePower = true;
	public bool powerOn = false;

	void Start () {
		
	}
	
	void Update () {
		if (!powerOn && infinitePower) 
			powerOn = !powerOn;

		if (powerOn){
			
		}
	}
}
