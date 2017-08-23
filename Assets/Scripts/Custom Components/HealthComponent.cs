using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour {

	[Header("Health Settings")]
	public float HealthMax = 100;
	public float HealthCurrent;

	[Header("Armor Settings")]
	public float ArmorMax = 100;
	public float ArmorCurrent;
	
	void Start(){
		HealthCurrent = HealthMax;
	}

	public void Damage(float hpval){
		HealthCurrent -= hpval;
		if (HealthCurrent < 0) HealthCurrent = 0;

	}
}
