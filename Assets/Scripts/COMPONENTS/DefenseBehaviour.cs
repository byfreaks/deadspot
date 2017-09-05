using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBehaviour : MonoBehaviour {

	HealthComponent hp;

	void Start () {
		hp = this.gameObject.GetComponent<HealthComponent>();
	}
	
	public void Damage(float d){
		if (hp.HealthCurrent <= 0){
			Break();
		} else {
			hp.HealthCurrent -= d;
			if (hp.HealthCurrent < 0)
				hp.HealthCurrent = 0;
		}
	}

	void Break(){
		Object.Destroy(this.gameObject);
	}
}
