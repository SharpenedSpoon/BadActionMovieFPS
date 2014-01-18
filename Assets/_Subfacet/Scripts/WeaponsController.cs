using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class WeaponsController : MonoBehaviour {

	public new static WeaponsController active;

	void Awake() {
		active = this;
	}

	void Start () {
	
	}

	void Update () {
	
	}
}
