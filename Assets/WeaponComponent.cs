using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponComponent : MonoBehaviour {

	private MainScriptPlayer Kira;
	public Material bulletMaterial;
	private Vector3 offsetPosition;
	public GameObject DBS;
	public WeaponDatabase wpn;
	public int currentWeaponID;

	private Weapon currentWeapon;

	enum weapon{
		hands = 1,
		pistol = 2

	}

	void Start(){
		wpn = DBS.GetComponent<WeaponDatabase>();
		Kira = GetComponent<MainScriptPlayer>();
		currentWeaponID = (int)weapon.pistol;
	}

	public void Shoot(Vector3 dir, float defaultMargin = 0, float maxDistance = 500f){

		currentWeapon = wpn.GetWeapon(currentWeaponID); //Get Current Weapon

		//Mouse and playerTorso position variables.
		Vector3 mousePos = OffsetCalculator.GetMousePos();
		Vector3 pos = Kira.playerTorso.transform.position + new Vector3(0,Kira.heightOffsetY,Kira.playerTorso.transform.position.z);
		RaycastHit2D hit; //raycast to shoot
		float rayMargin;
		Vector3 rayDirection;
		float rayDistance = 0f;
		float ang;
		Vector3 lineOffset;

		switch(currentWeapon.WeaponType){
			case 2: //SingleFire weapons

				if (Kira.justShot == true)
					break;

				//Ray Propierties
				rayMargin = Random.Range(-defaultMargin,defaultMargin);
				rayDirection = (new Vector3(0, rayMargin, 0) + dir);
				rayDistance = maxDistance;

				//Calculate Offset
				ang = Vector2.Angle(Vector2.right,dir); 
				offsetPosition = OffsetCalculator.GetOffsetPosition(mousePos.y > pos.y, pos, ang, Kira.lengthOffsetX, Kira.lengthOffsetY);

				//Shoot
				hit = Physics2D.Raycast( pos, rayDirection, rayDistance, Kira.shootingLayer);
				Debug.DrawRay(pos, rayDirection * rayDistance, Color.blue);

				//Cosmetic
				lineOffset = OffsetCalculator.GetOffsetPosition(mousePos.y > pos.y, pos, ang, Kira.shotLength, Kira.shotLength);

				if (hit.collider != null) // Si hit.point es un zombie 
					DrawLine(offsetPosition, hit.point ,Color.white, 0.1f);
				else 
					DrawLine(offsetPosition, lineOffset ,Color.white, 0.1f);

				if (hit.collider != null){
					
					//Debug Actual hit
					Debug.DrawRay(offsetPosition,hit.point - (Vector2)offsetPosition,Color.red,0);

					Rigidbody2D hitRb = hit.collider.GetComponent<Rigidbody2D>();
					if (hitRb.bodyType != RigidbodyType2D.Static){
						hitRb.velocity = new Vector2(30,0); //temporal kickback targert	
						hit.collider.GetComponent<HealthComponent>().Damage(50); //placeholder damager
					}
					
				}

				Kira.justShot = true;
				break;

			case 3: //autoWeapons

				//Ray Propierties
				rayMargin = Random.Range(-defaultMargin,defaultMargin);
				rayDirection = (new Vector3(0, rayMargin, 0) + dir);
				rayDistance = maxDistance;

				//Calculate Offset
				ang = Vector2.Angle(Vector2.right,dir); 
				offsetPosition = OffsetCalculator.GetOffsetPosition(mousePos.y > pos.y, pos, ang, Kira.lengthOffsetX, Kira.lengthOffsetY);

				//Shoot
				hit = Physics2D.Raycast( pos, rayDirection, rayDistance, Kira.shootingLayer);
				Debug.DrawRay(pos, rayDirection * rayDistance, Color.blue);

				//Cosmetic
				lineOffset = OffsetCalculator.GetOffsetPosition(mousePos.y > pos.y, pos, ang, Kira.shotLength, Kira.shotLength);

				if (hit.collider != null) // Si hit.point es un zombie 
					DrawLine(offsetPosition, hit.point ,Color.white, 0.1f);
				else 
					DrawLine(offsetPosition, lineOffset ,Color.white, 0.1f);

				if (hit.collider != null){
					
					//Debug Actual hit
					Debug.DrawRay(offsetPosition,hit.point - (Vector2)offsetPosition,Color.red,0);

					Rigidbody2D hitRb = hit.collider.GetComponent<Rigidbody2D>();
					if (hitRb.bodyType != RigidbodyType2D.Static){
						hitRb.velocity = new Vector2(30,0); //temporal kickback targert	
						hit.collider.GetComponent<HealthComponent>().Damage(50); //placeholder damager
					}
					
				}
				break;
		}

		
		
	}

	void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.1f){
		GameObject myLine = new GameObject();
		myLine.transform.position = start;
		myLine.AddComponent<LineRenderer>();
		LineRenderer lr = myLine.GetComponent<LineRenderer>();
		//lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
		lr.material = bulletMaterial;
		lr.startColor = color;
		lr.endColor = color;
		lr.startWidth = 0.1f;
		lr.endWidth = 1.2f;
		lr.SetPosition(0, start);
		lr.SetPosition(1, end);
		GameObject.Destroy(myLine, duration);
	}

	void OnDrawGizmos(){
		Gizmos.DrawSphere(offsetPosition, 2);
	}

}

