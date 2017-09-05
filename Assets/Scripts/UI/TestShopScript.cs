using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestShopScript : MonoBehaviour {

	public bool openNow = true;
	public GameObject inputObject;
	public GameObject shopPanel;
	public GameObject player;
	public GameObject baseButton;
	public GameObject weaponDatabase;
	public List<Weapon> inStoreItems;
	public GameObject inventoryPanel;
	public GameObject openButton;

	public GameObject parent;
	
	private List<GameObject> buttons{ get; set; }
	private InputController inputer;

	void Start(){
		buttons = new List<GameObject>();
		buttons.Add( CreateButton(2) );
		buttons.Add( CreateButton(3) );
	}

	void myFunction(string str){
		Debug.Log("Bought a "+str);
	}

	GameObject CreateButton(int id){
		//GameObject myButton = new GameObject();
		//Weapon weapon = weaponDatabase.GetComponent<WeaponDatabase>().Database.Find(x => x.ID == id); 
		Weapon weapon = weaponDatabase.GetComponent<WeaponDatabase>().GetWeapon(id); 

		GameObject myButton = Instantiate(baseButton);
		//myButton.transform.parent = this.transform;
		myButton.transform.SetParent(parent.transform,false);
		myButton.transform.Find("Text").GetComponent<Text>().text = weapon.Name;
		myButton.GetComponent<Button>().onClick.AddListener( () => inventoryPanel.GetComponent<InventoryScript>().AddToInventory(id) );
		

		myButton.transform.position += new Vector3(0,-35 * (buttons.Count+1) ,0);

		return myButton;
	}

	public void ToogleOpen(){
		openNow = !openNow;
	}

	void Update(){
		if (openNow){
			shopPanel.SetActive(true);
			openButton.SetActive(false);
			

		} else {
			shopPanel.SetActive(false);
			openButton.SetActive(true);
		}

	}

	
}
