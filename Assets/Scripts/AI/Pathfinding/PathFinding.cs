using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour {

	public int playerZone;

	//Zones Connection Objects
	public GameObject[] zoneConnectionObj; 

	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public GameObject findConnection(int currentZone){

		foreach (var item in zoneConnectionObj)
		{
			if(item.GetComponent<ValuesZCObj>().zoneOne == currentZone || item.GetComponent<ValuesZCObj>().zoneTwo == currentZone ){
				if(item.GetComponent<ValuesZCObj>().zoneTwo == playerZone || item.GetComponent<ValuesZCObj>().zoneOne == playerZone){
					return item;
				}
			}
		}

		foreach (var item in zoneConnectionObj)
		{
			if(item.GetComponent<ValuesZCObj>().zoneOne == currentZone || item.GetComponent<ValuesZCObj>().zoneTwo == currentZone ){
				return item;
			}
		}
		
		return null;
		
	}


}
