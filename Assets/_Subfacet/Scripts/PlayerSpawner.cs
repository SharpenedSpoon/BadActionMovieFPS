using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {

	public Vector3 playerSpawnLocation = new Vector3(130, 0, 170);
	public GameObject player;
	public GameObject secondaryCamera;

	public bool resetGameOnPlayerDeath = true;

	private GameObject currentPlayer = null;

	void Start () {
		currentPlayer = GameObject.FindGameObjectWithTag("Player");
		if (currentPlayer == null) {
			SpawnPlayer();
		}
	}

	void Update () {
		if (currentPlayer == null) {
			secondaryCamera.SetActive(true);
			if (Input.GetKeyDown(KeyCode.R)) {
				SpawnPlayer();
				secondaryCamera.SetActive(false);
			}
		}
	}

	public void SpawnPlayer() {
		Vector3 spawnLocation = playerSpawnLocation;
		spawnLocation.x = spawnLocation.x + Random.Range(-25.0f, 25.0f);
		spawnLocation.z = spawnLocation.z + Random.Range(-25.0f, 25.0f);
		spawnLocation.y = Terrain.activeTerrain.SampleHeight(spawnLocation) + 1.0f;
		currentPlayer = Instantiate(player, spawnLocation, Quaternion.identity) as GameObject;

		if (resetGameOnPlayerDeath) {
			EnemySpawner.active.ResetWaves();
		}
	}
}
