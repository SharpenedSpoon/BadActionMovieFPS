using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {

	public Vector3 playerSpawnLocation = new Vector3(130, 2, 170);
	public GameObject player;
	public GameObject secondaryCamera;

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
			if (Input.anyKeyDown) {
				SpawnPlayer();
				secondaryCamera.SetActive(false);
			}
		}
	}

	public void SpawnPlayer() {
		currentPlayer = Instantiate(player, playerSpawnLocation, Quaternion.identity) as GameObject;
	}
}
