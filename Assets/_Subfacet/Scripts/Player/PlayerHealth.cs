using UnityEngine;
using System.Collections;

public class PlayerHealth : HasHealth {

	protected override void Die() {
		if (LeaderboardController.active) {
			LeaderboardController.active.AddScore();
		}

		base.Die();
	}

}
