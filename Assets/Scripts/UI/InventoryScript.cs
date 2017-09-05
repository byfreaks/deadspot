using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour {

	public GameObject player;
	public GameObject baseButton;

	private List<GameObject> buttons = new List<GameObject>();

	void Start () {
		//buttons.Add( CreateButton(3) );
	}

	public void AddToInventory(int id){
		WeaponComponent wpn = player.GetComponent<WeaponComponent>();
		wpn.WeaponInventory.Add( wpn.AddWeapon(id) ); //FIXME: make proper-er
		WeaponItem item = wpn.WeaponInventory.Find(x => x.Weapon.ID == id);
		item.Ammo = 100;

		if (item != null)
			//if player has a waponitem correspondant of the idea
			buttons.Add( CreateButton(id, item, wpn) );
		else
			Debug.Log("Attempted to access WeaponItem the player does not have!");
	}

	GameObject CreateButton(int id, WeaponItem item, WeaponComponent wpn){
		
		GameObject myButton = Instantiate(baseButton);
		myButton.transform.SetParent(this.transform,false);
		myButton.transform.Find("Text").GetComponent<Text>().text = item.Weapon.Name;
		myButton.GetComponent<Button>().onClick.AddListener( () => wpn.SwapWeaponDev(id) );

		myButton.transform.position += new Vector3(60 * (buttons.Count+1) , 0, 0);
		Debug.Log(buttons.Count);
		return myButton;

	}
}
