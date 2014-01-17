using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public new static EnemySpawner active;

	void Awake() {
		active = this;
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
