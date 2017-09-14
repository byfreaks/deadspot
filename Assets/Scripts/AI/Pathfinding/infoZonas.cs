using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infoZonas : MonoBehaviour{

	//Pathfinding
	public int numberZone;
	public GameObject pathController;
	private PathFinding scriptPath;

	//Player
	public GameObject Player;
	private Collider2D plyCol;
	// Use this for initialization
	void Start () {
		if(Player!=null){
			plyCol = Player.GetComponent<Collider2D>();
		}
		scriptPath = pathController.GetComponent<PathFinding>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other == plyCol){
			scriptPath.updateZone(this.numberZone);
		}
		if(other.gameObject.tag =="Enemy"){
			other.gameObject.GetComponent<MainScriptAI>().currentZone = numberZone;
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if(other == plyCol){
			scriptPath.playerZone = this.numberZone;
		}
		if(other.gameObject.tag =="Enemy"){
			other.gameObject.GetComponent<MainScriptAI>().currentZone = numberZone;
		}
	}
}
