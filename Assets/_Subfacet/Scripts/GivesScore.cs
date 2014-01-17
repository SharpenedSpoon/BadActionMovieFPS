using UnityEngine;
using System.Collections;

public class GivesScore : MonoBehaviour {

	public bool givesScoreOnDeath = true;
	public int scoreOnDeath = 10;

	public bool givesMoneyOnDeath = true;
	public int moneyOnDeath = 100;

	public bool givesScoreOnHit = true;
	public int scoreOnHit = 1;

	public bool givesMoneyOnHit = true;
	public int moneyOnHit = 1;

	private ScoreController scoreController;

	void Start () {
		scoreController = ScoreController.active;
		if (! scoreController) {
			enabled = false;
		}
	}

	public void DeathOccured() {
		if (givesScoreOnDeath) {
			AddPoints(scoreOnDeath);
		}
		if (givesMoneyOnDeath) {
			AddMoney(moneyOnDeath);
		}
	}

	public void GotShot() {
		if (givesScoreOnHit) {
			AddPoints(scoreOnHit);
		}
		if (givesMoneyOnHit) {
			AddMoney(moneyOnHit);
		}
	}

	private void AddPoints(int pts) {
		scoreController.AddPoints(pts);
	}

	private void AddMoney(int money) {
		scoreController.AddMoney(money);
	}
}
