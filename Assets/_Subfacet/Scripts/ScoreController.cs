using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreController : MonoBehaviour {

	public new static ScoreController active;

	public int points { get; private set; }
	public int money { get; private set; }

	private Dictionary<float, int> pointHistory = new Dictionary<float, int>();
	private Dictionary<float, int> moneyHistory = new Dictionary<float, int>();
	private Dictionary<float, int> pointSumHistory = new Dictionary<float, int>();
	private Dictionary<float, int> moneySumHistory = new Dictionary<float, int>();

	void Awake() {
		active = this;
	}

	public void AddPoints(int pts) {
		points += pts;
		pointHistory.Add(Time.time, pts);
		pointSumHistory.Add(Time.time, points);
	}

	public void AddMoney(int dollahs) {
		money += dollahs;
		moneyHistory.Add(Time.time, dollahs);
		moneySumHistory.Add(Time.time, money);
	}

	public void SpendMoney(int dollahs) {
		money -= dollahs;
		moneyHistory.Add(Time.time, -1*dollahs);
		moneySumHistory.Add(Time.time, money);
	}
}
