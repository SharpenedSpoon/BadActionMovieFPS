using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class WeaponsController : MonoBehaviour {

	public List<WeaponData> weapons = new List<WeaponData>();

	public new static WeaponsController active;

	void Awake() {
		active = this;
	}

	void Start() {
		string weaponsString = FileIO.ReadFromFile("weapons.txt");
		if (weaponsString != null && weaponsString != "" && weaponsString != "{}") {
			weapons = JsonConvert.DeserializeObject<List<WeaponData>>(weaponsString);
		}
	}

}
