using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;

using UnityEngine;

public class WeaponDatabase : MonoBehaviour {

	private List<Weapon> database = new List<Weapon>();
	private JsonData weaponData;

	public List<Weapon> Database{ 
		get{
			return database;
		}
		set{
			database = value;
		}
	}

	void Start () {
		weaponData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Weapons.json"));
		ConstructWeaponDatabase();
	}

	void ConstructWeaponDatabase(){
		for (int i = 0; i < weaponData.Count; i++ ){
			database.Add(new Weapon(
				(int)weaponData[i]["id"],
				weaponData[i]["name"].ToString(),
				(int)weaponData[i]["type"],
				float.Parse(weaponData[i]["x_offset"].ToString()),
				float.Parse(weaponData[i]["y_offset"].ToString()),
				(int)weaponData[i]["magsize"]

				
			) ); //close database.Add
		}
		Debug.Log("Weapon Database constructed");
	}

	public Weapon GetWeapon(int id){
		return Database.Find(x => x.ID == id);
		//return database[id];
	}
	
}

public class Weapon{
	public int ID {get; set;}
	public string Name {get; set;}
	public int WeaponType {get; set;}
	public float OriginOffsetX {get; set;}
	public float OriginOffsetY {get; set;}
	public int Magsize {get; set;}
	
	public Weapon(int id, string name, int type, float offsetX, float offsetY, int mag){
		this.ID = id;
		this.Name = name;
		this.WeaponType = type;
		this.OriginOffsetX = offsetX;
		this.OriginOffsetY = offsetY;
		this.Magsize = mag;
		
	}
	
}