using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	public new static Score active;

	void Awake() {
		active = this;
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
