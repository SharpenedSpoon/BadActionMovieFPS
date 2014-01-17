using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemyTemplate;
	public GUIText enemyGuiInfo;

	public int wave { get; private set; }
	
	private List<GameObject> enemies = new List<GameObject>();
	[HideInInspector]
	public int enemyCount {
		get {
			return enemies.Count;
		}
		set {
			// cannot set
		}
	}

	public new static EnemySpawner active;


	void Awake() {
		active = this;
		wave = 1;
	}

	void Start () {
		FindAllEnemies();
	}

	void Update () {
		// remove dead enemies from the list
		enemies.RemoveAll(item => item == null);

		if (Input.GetKeyDown(KeyCode.N)) {
			SpawnEnemies(10);
			FindAllEnemies();
		}
	}

	private void SpawnEnemies(int num) {
		for (int i=0; i<num; i++) {
			Vector3 thisSpawnPosition = new Vector3(100, 2, 150);
			Instantiate(enemyTemplate, thisSpawnPosition, Quaternion.identity);
		}
	}

	private void FindAllEnemies() {
		enemies = new List<GameObject>();
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
			enemies.Add(enemy);
		}
	}
}
