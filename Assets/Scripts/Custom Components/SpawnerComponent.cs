using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerComponent : MonoBehaviour {

	enum md{
		interval,
		plan
	}
	

	//General Vars
	[Header("General Options")]
	public bool enabled;
	public GameObject toSpawn;
	public GameObject dynamicObjectsParent;
	public int mode = (int)md.interval;

	[Header("Interval Mode Options")]
	//Interval Vars
	public double timeInterval;
	public int instancesPerSpawn;
	public bool isInfinite = false;
	public int quantity;

	int timeSinceCreate = 0;
	int timeSpeed = 1;
	int time;

	//Plan vars


	void Start () {
		//Settings
		timeInterval = 150;

	}
	

	void Update () {
		switch (mode){
			case 0:
				
				if (time >= timeInterval && enabled){
					time = 0;

					if (isInfinite){
						Spawn(instancesPerSpawn);
					} else {
						if (quantity > 0){
							Spawn(instancesPerSpawn);
							quantity-=instancesPerSpawn;
						}
					}

				} else {
					time+=timeSpeed;
				}

				break;


		}
	}

	void Spawn(int amount = 1){
		while(amount > 0){
			if (amount <= 0) break;
			GameObject myObject = Instantiate(toSpawn, this.transform.position,Quaternion.identity);
			myObject.transform.parent = dynamicObjectsParent.transform;
			amount--;
		}
	}
}
