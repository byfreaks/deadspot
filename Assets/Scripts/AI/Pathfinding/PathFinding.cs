using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour {

	public int playerZone;
	public int prevPlayerZone;
	public int prevPlayerZone2;

	//Zones Connection Objects
	public GameObject[] zoneConnectionObj; 

	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public GameObject findConnection(int currentZone){
		ValuesZCObj aux;
		foreach (var item in zoneConnectionObj)
		{
			aux = item.GetComponent<ValuesZCObj>(); 
			if(aux.zoneOne == currentZone || aux.zoneTwo == currentZone ){
				Debug.Log("Primero");
				if(aux.zoneTwo == playerZone || aux.zoneOne == playerZone){
					Debug.Log("Primero Return");
					return item;
				}
			}
		}

		foreach (var item in zoneConnectionObj)
		{
			aux = item.GetComponent<ValuesZCObj>(); 
			if(aux.zoneOne == currentZone || aux.zoneTwo == currentZone ){
				Debug.Log("Segundo");
				if(aux.zoneTwo == prevPlayerZone || aux.zoneOne == prevPlayerZone){
					Debug.Log("Segundo Return");
					return item;
				}
			}
		}

		foreach (var item in zoneConnectionObj)
		{
			aux = item.GetComponent<ValuesZCObj>();
			if(aux.zoneOne == currentZone || aux.zoneTwo == currentZone ){
				Debug.Log("TErcero");
				if(aux.zoneTwo == prevPlayerZone2 || aux.zoneOne == prevPlayerZone2){
					Debug.Log("Tercero Return");
					return item;
				}
			}
		}


		Debug.Log("FUCKKSDLAD");
		return null;
		
	}


	public void updateZone(int currentZone){
		this.prevPlayerZone2 = this.prevPlayerZone;
		this.prevPlayerZone = this.playerZone;
		this.playerZone = currentZone;
}
}
