using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;

public class ScoreController : MonoBehaviour {

	public int points { get; private set; }
	public int money { get; private set; }

	private List<float> pointTimes = new List<float>();
	private List<float> moneyTimes = new List<float>();
	private List<int> pointHistory = new List<int>();
	private List<int> moneyHistory = new List<int>();
	private List<int> pointSumHistory = new List<int>();
	private List<int> moneySumHistory = new List<int>();

	public new static ScoreController active;

	void Awake() {
		active = this;
	}

	public void AddPoints(int pts) {
		points += pts;
		float currentTime = Time.time;
		pointTimes.Add(currentTime);
		pointHistory.Add(pts);
		pointSumHistory.Add(points);
	}

	public void AddMoney(int dollahs) {
		money += dollahs;
		float currentTime = Time.time;
		moneyTimes.Add(currentTime);
		moneyHistory.Add(dollahs);
		moneySumHistory.Add(money);
	}

	public void SpendMoney(int dollahs) {
		money -= dollahs;
		float currentTime = Time.time;
		moneyTimes.Add(currentTime);
		moneyHistory.Add(-1*dollahs);
		moneySumHistory.Add(money);
	}

	public void ResetScore() {
		points = 0;
		money = 0;

		pointTimes = new List<float>();
		moneyTimes = new List<float>();
		pointHistory = new List<int>();
		moneyHistory = new List<int>();
		pointSumHistory = new List<int>();
		moneySumHistory = new List<int>();
	}

	public bool CanAfford(int itemCost) {
		return (money >= itemCost);
	}
}