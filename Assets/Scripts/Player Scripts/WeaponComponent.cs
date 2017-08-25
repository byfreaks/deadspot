using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon {

	public class WeaponType{

		string gunName;
		int index;

		public string GunName{
			set{ gunName = value; }
			get{ return gunName; }

		}

		public int Index{
			set{
				var valString = value.ToString();
				if (valString.Length > 2){

				}
			}
			get{
				return index;
			}
		}

		

	}

	public class WeaponComponent : MonoBehaviour {

		public WeaponType pistol;

		void Start () {
			pistol = new WeaponType();
			pistol.GunName = "Pistol";
			pistol.Index = 01;	
		}
		
	} 
}

