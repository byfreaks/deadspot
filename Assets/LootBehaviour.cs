using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBehaviour : MonoBehaviour {

	private float speed = 1.7f;
	private float amplitude = 0.1f;
	private int itemID = 0;

	void FixedUpdate(){
		float y0 = transform.position.y;
		this.transform.position = new Vector2(transform.position.x, y0+amplitude*Mathf.Sin(speed*Time.time) );
	}
}
