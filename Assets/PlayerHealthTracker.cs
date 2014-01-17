using UnityEngine;
using System.Collections;

public class PlayerHealthTracker : MonoBehaviour {

	public GUIText playerHealthText = null;

	private HasHealth playerHealth;

	void Start () {
		if (playerHealthText == null) {
			playerHealthText = GetComponent<GUIText>();
			if (playerHealthText == null) {
				enabled = false;
				return;
			}
		}

		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if (player == null) {
			enabled = false;
			return;
		}

		playerHealth = player.GetComponent<HasHealth>();
		if (playerHealth == null) {
			enabled = false;
			return;
		}
	}

	void Update () {
		playerHealthText.text = "HP: " + playerHealth.health;
	}
}
