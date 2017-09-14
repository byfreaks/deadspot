using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValuesZCObj: MonoBehaviour {

	public int zoneOne;
	public int zoneTwo;
	public float positionOne;
	public float positionTwo;
	public GameObject objectPosOne;
	public GameObject objectPosTwo;

	void Update(){
		this.positionOne = this.objectPosOne.transform.position.y;
		this.positionTwo = this.objectPosTwo.transform.position.y;
	}
}
