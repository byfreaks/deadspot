using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponComponent : MonoBehaviour {

	private MainScriptPlayer Kira;
	public Material bulletMaterial;
	private Vector3 offsetPosition;
	public GameObject DBS;
	[HideInInspector] public WeaponDatabase wpn; 
	public int currentWeaponID;
	private Weapon currentWeapon;

	public WeaponItem currentWeaponItem;

	//public WeaponItem[] WeaponInventory = new WeaponItem[5];
	public List<WeaponItem> WeaponInventory = new List<WeaponItem>();


	enum weapon{
		hands = 1,
		pistol = 2

	}

	void Start(){
		wpn = DBS.GetComponent<WeaponDatabase>();
		Kira = GetComponent<MainScriptPlayer>();
		currentWeaponID = (int)weapon.hands;

		//WeaponInventory[0] = AddWeapon(1); //hands

		WeaponInventory.Add( AddWeapon(1) );

		// WeaponInventory.Add( AddWeapon(2) );
		// WeaponInventory[1].Ammo = 150;
		// WeaponInventory[1].Chamber = 0;

		// WeaponInventory.Add( AddWeapon(3) );
		// WeaponInventory[2].Ammo = 400;
		// WeaponInventory[2].Chamber = 0;

		SwapWeaponDev(1);
	}

	void Update(){
		if (currentWeaponItem != null){
			string toDisplayChamber;
			if (currentWeaponItem.Weapon.Name != "Hands")
				toDisplayChamber = currentWeaponItem.Chamber+"/"+currentWeaponItem.Weapon.Magsize+"  |  "+currentWeaponItem.Ammo;
			else
				toDisplayChamber = "Unarmed";
			GameObject.Find("ChamberUI").GetComponent<Text>().text = toDisplayChamber;
		}
	}

	public WeaponItem AddWeapon(int id){
		return new WeaponItem(wpn.GetWeapon(id));
	}

	public void SwapWeaponDev(int w){
		currentWeaponID = w;
		switch(w){
			case 1:
				currentWeaponItem = WeaponInventory[0];
				break;
			case 2:
				currentWeaponItem = WeaponInventory[1];
				break;
			case 3:
				currentWeaponItem = WeaponInventory[2];
				break;
		}
		Debug.Log("Switched to: " + currentWeaponItem.Weapon.Name + " Type is: "+currentWeaponItem.Weapon.WeaponType);
	}

	public void Shoot(Vector3 dir, float defaultMargin = 0, float maxDistance = 500f){

		//currentWeapon = wpn.GetWeapon(currentWeaponID); //Get currentWeaponItem Weapon
		currentWeapon = currentWeaponItem.Weapon;

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

				if (currentWeaponItem.Chamber <= 0)
					break;
				else 
					currentWeaponItem.Chamber--;

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

				if (currentWeaponItem.Chamber <= 0)
					break;
				else 
					currentWeaponItem.Chamber--;

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

public class WeaponItem {

	private Weapon weapon;
	private int ammo;
	private int chamber;

	public Weapon Weapon {get; set;} 
	public int Ammo {
		get{ return ammo; } 
		set{
			if (value >= 0) ammo = value;
			else Debug.Log("Z> Trying to store a negative ammo number!");
		}
	}
	public int Chamber {
		get{ return chamber;  } 
		set{ chamber = value; }
	}


	public WeaponItem(){}

	public WeaponItem(Weapon w){
		this.Weapon = w;
	}

	public void SingleFire(){
		if (Chamber > 0) Chamber--;
	}

	public void Reload(){
		if (Ammo > 0 && Chamber < Weapon.Magsize){
			// if (Ammo > Weapon.Magsize){
			// 	Chamber += Weapon.Magsize;
			// 	Ammo -= Chamber;
			// 	Debug.Log("caso 1");
			// } else {
			// 	Chamber += Ammo-Chamber;
			// 	Ammo -= Weapon.Magsize;
			// 	Debug.Log("caso 2");
			// }

			int missingBullets = Weapon.Magsize - Chamber;
			
			if (Ammo >= missingBullets){
				Chamber += missingBullets;
				Ammo -= missingBullets;
				return;
			} else {
				if (Ammo + Chamber <= Weapon.Magsize){
					Chamber += Ammo;
					Ammo = 0;
				}
			}

		}
	}
	
}