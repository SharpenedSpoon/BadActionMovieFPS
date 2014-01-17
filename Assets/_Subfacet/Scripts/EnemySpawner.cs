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
	}

	void Start () {
		ResetWaves();
		StartCoroutine(WaveSpawningCoroutine());
	}

	void Update () {
		// remove dead enemies from the list
		enemies.RemoveAll(item => item == null);

		if (Input.GetKeyDown(KeyCode.N)) {
			SpawnEnemies(10);
		}
	}

	public void ResetWaves() {
		wave = 1;
		foreach (GameObject enemy in enemies) {
			Destroy(enemy);
		}
		SpawnEnemies(10);
	}

	private IEnumerator WaveSpawningCoroutine() {
		while (true) {
			if (enemies.Count == 0) {
				SpawnEnemies(10);
				wave += 1;
			}
			yield return new WaitForSeconds(7.0f);
		}
	}

	private void SpawnEnemies(int num) {
		for (int i=0; i<num; i++) {
			Vector3 thisSpawnPosition = new Vector3(100, 2, 150);
			thisSpawnPosition.x = thisSpawnPosition.x + Random.Range(-50.0f, 50.0f);
			thisSpawnPosition.z = thisSpawnPosition.z + Random.Range(-50.0f, 50.0f);
			thisSpawnPosition.y = Terrain.activeTerrain.SampleHeight(thisSpawnPosition) + 1.0f;
			Instantiate(enemyTemplate, thisSpawnPosition, Quaternion.identity);
		}
		
		FindAllEnemies();
	}

	private void FindAllEnemies() {
		enemies = new List<GameObject>();
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
			enemies.Add(enemy);
		}
	}
}
