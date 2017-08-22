using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (player != null) 
			this.transform.position = new Vector3(player.transform.position.x,this.transform.position.y,-10);
		
	}
}
