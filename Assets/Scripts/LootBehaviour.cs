using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBehaviour : MonoBehaviour {

	private float speed = 1.7f;
	private float amplitude = 0.1f;
	private int itemID = 0;
	public int ItemID{
		set{
			if (itemID == 0 && value != 0){
				itemID = value;
			}
		}
		get{ return itemID; }
	}

	public int value;
	public GameObject spr;

	void Start(){
		value = Random.Range(0,Mathf.CeilToInt(999));
	}

	void FixedUpdate(){
		float y0 = spr.transform.position.y;
		spr.transform.position = new Vector2(spr.transform.position.x, y0+amplitude*Mathf.Sin(speed*Time.time) );
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.transform.name == "PlayerKira"){
			Object.Destroy(this.gameObject);
			coll.GetComponent<MainScriptPlayer>().valueCollected+=value;
		}
	}
}
