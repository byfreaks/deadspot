using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;

using UnityEngine;

public class WeaponDatabase : MonoBehaviour {

	private List<Weapon> database = new List<Weapon>();
	private JsonData weaponData;

	void Start () {
		weaponData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Weapons.json"));
		ConstructWeaponDatabase();

		Debug.Log(database[1].Name);
	}

	void ConstructWeaponDatabase(){
		for (int i = 0; i < weaponData.Count; i++ ){
			database.Add(new Weapon((int)weaponData[i]["id"], weaponData[i]["name"].ToString(), (int)weaponData[i]["type"] ));
		}
	}

	public string ReturnName(int id){
		return database[id].Name;
	}

	public int GetType(int id){
		return database[id].WeaponType;
	}

	public Weapon GetWeapon(int id){
		return database[id];
	}
	
}

public class Weapon{
	public int ID {get; set;}
	public string Name {get; set;}
	public int WeaponType {get; set;}

	public Weapon(int id, string name, int type){
		this.ID = id;
		this.Name = name;
		this.WeaponType = type;
	}
	
}