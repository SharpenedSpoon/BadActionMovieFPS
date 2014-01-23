using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Radar : MonoBehaviour {

	public float maxDistance = 40.0f;
	public int radarPixelSize = 128;

	public Texture radarBackground;
	public Texture targetBlip;

	private EnemySpawner enemySpawner = null;
	private PlayerSpawner playerSpawner = null;

	void Start () {
		if (EnemySpawner.active == null || PlayerSpawner.active == null) {
			enabled = false;
			return;
		}

		enemySpawner = EnemySpawner.active;
		playerSpawner = PlayerSpawner.active;
	}

	void OnGUI() {
		Debug.Log("hi!");
		if (playerSpawner.currentPlayer == null) {
			return;
		}
		Rect radarBackgroundRect = new Rect(0, 0, radarPixelSize, radarPixelSize);
		GUI.DrawTexture(radarBackgroundRect, radarBackground);

		Transform playerTransform = playerSpawner.currentPlayer.transform;
		foreach (GameObject go in enemySpawner.enemies) {
			float distanceToTarget = Vector3.Distance(go.transform.position, playerTransform.position);
			if (distanceToTarget < maxDistance) {
				DrawBlip(playerTransform, go.transform, distanceToTarget);
			}
		}
	}

	private void DrawBlip(Transform playerTransform, Transform enemyTransform, float distance) {
		Vector3 dist = enemyTransform.position - playerTransform.position;
		dist.y = 0;
		float angleToTarget = Mathf.Atan2(dist.x, dist.z) * Mathf.Rad2Deg;

		float playerFacingAngle = playerTransform.eulerAngles.y;

		float radarAngle = Mathf.Deg2Rad * (angleToTarget - playerFacingAngle - 90.0f);

		Vector2 blipCoordinates = new Vector2((distance / maxDistance) * (Mathf.Cos(radarAngle)), (distance / maxDistance) * (Mathf.Sin(radarAngle)));

		// scale coordinates
		blipCoordinates = 0.5f * radarPixelSize * blipCoordinates;

		// offset coordinates relative to center
		blipCoordinates = (0.5f * radarPixelSize) * Vector2.one + blipCoordinates;

		Rect blipRect = new Rect(blipCoordinates.x - 2, blipCoordinates.y - 2, 4, 4);
		GUI.DrawTexture(blipRect, targetBlip);
	}
}
