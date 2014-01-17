using UnityEngine;
using System.Collections;

public class ExplosionController : MonoBehaviour {

	public new static ExplosionController active;

	void Awake() {
		active = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
