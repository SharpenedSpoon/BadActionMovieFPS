using UnityEngine;
using System.Collections;

public class GivesScore : MonoBehaviour {

	public bool givesScoreOnDeath = true;
	public int scoreOnDeath = 1;

	public bool givesMoneyOnDeath = true;
	public int moneyOnDeath = 60;

	public bool givesScoreOnHit = false;
	public int scoreOnHit = 0;

	public bool givesMoneyOnHit = true;
	public int moneyOnHit = 10;

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
