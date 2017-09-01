using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponComponent : MonoBehaviour {

	enum wpn{
		hands = 0,
		pistol = 1
	}

	public int weapon = (int)wpn.hands;

	void Start () {
		
	}
	
	void Update () {
		switch (weapon){
			case 1:

				break;

		}
	}
}
