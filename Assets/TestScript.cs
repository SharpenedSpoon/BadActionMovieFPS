using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {

	public Color col = Color.black;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		renderer.material.color = col;
	}
}
