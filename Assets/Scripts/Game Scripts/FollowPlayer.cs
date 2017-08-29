using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

	public GameObject player;
	public bool zoomEnabled = true;
	public float zoomSize = 2.5f; 

	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
		if (player != null){
			//this.transform.position = new Vector3(player.transform.position.x,this.transform.position.y,-10);
			//TODO: make it less terrible
			if (player.GetComponent<MainScriptPlayer>().isAiming && zoomEnabled){
				Vector3 plypos = player.transform.position;
				Vector3 mousePos = OffsetCalculator.GetMousePos();
				float angle = Vector2.Angle(Vector2.right,mousePos-plypos);
				Vector3 aimPos = OffsetCalculator.GetOffsetPosition(plypos.y < mousePos.y,plypos,angle,zoomSize, zoomSize);

				Debug.DrawRay(plypos,mousePos-plypos,Color.cyan);

				this.transform.position = new Vector3(aimPos.x,aimPos.y,-10);
			} else {
				this.transform.position = new Vector3(player.transform.position.x,player.transform.position.y,-10);
			}
			
		}
	}
}
