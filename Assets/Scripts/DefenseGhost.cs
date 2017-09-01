using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseGhost : MonoBehaviour {

	public GameObject obj;
	public GameObject parentObj;
	private SpriteRenderer spriteRenderer;

	void Start(){
		spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
		parentObj = GameObject.Find("DynamicObstacles");
	}
	void Update () {
		Vector2 mousePos = OffsetCalculator.GetMousePos();
		this.transform.position = mousePos;

		if (CanBuildHere()) spriteRenderer.color = new Color(0,1,0.29f,0.30f);
		else spriteRenderer.color = new Color(1,0,0,0.30f);
	}

	public void Create() {
		if (CanBuildHere()){
			GameObject myObj = Instantiate(obj,this.transform.position,Quaternion.identity);
			myObj.transform.parent = parentObj.transform;
			myObj.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 11);
			DestroyImmediate(this.gameObject);
		}
	}

	public bool CanBuildHere(){
		if (Physics2D.BoxCast(this.transform.position, spriteRenderer.bounds.size, 0, this.transform.position) == false ){
			return true;
		} else {
			return false;
		}
	}
}
