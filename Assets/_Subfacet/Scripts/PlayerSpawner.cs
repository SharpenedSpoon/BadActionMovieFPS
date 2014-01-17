using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {

	public Vector3 playerSpawnLocation = new Vector3(100, 0, 100);
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
			if (secondaryCamera) {
				secondaryCamera.SetActive(true);
			}
			if (Input.GetKeyDown(KeyCode.R)) {
				SpawnPlayer();
				if (secondaryCamera) {
					secondaryCamera.SetActive(false);
				}
			}
		}
	}

	public void SpawnPlayer() {
		Vector3 spawnLocation = playerSpawnLocation;
		spawnLocation.x = spawnLocation.x + Random.Range(-25.0f, 25.0f);
		spawnLocation.z = spawnLocation.z + Random.Range(-25.0f, 25.0f);
		spawnLocation.y = Terrain.activeTerrain.SampleHeight(spawnLocation) + 1.0f;
		currentPlayer = Instantiate(player, spawnLocation, Quaternion.identity) as GameObject;

		if (resetGameOnPlayerDeath && EnemySpawner.active) {
			EnemySpawner.active.ResetWaves();
			ScoreController.active.ResetScore();
		}
	}
}
