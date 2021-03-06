﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class LeaderboardController : MonoBehaviour {

	public List<ScoreData> scores = new List<ScoreData>();
	
	public new static LeaderboardController active;

	void Awake() {
		active = this;
	}

	void Start() {
		string jsonScores = FileIO.ReadFromFile("highscores.txt");
		if (jsonScores != null && jsonScores != "" && jsonScores != "{}") {
			scores = JsonConvert.DeserializeObject<List<ScoreData>>(jsonScores);
		}
	}

	public void AddScore() {
		ScoreData data = new ScoreData();
		data.money = ScoreController.active.money;
		data.score = ScoreController.active.points;
		data.wave = EnemySpawner.active.wave;
		AddScore(data);
	}

	public void AddScore(ScoreData data) {
		// append data to scores
		scores.Add(data);

		// serialize scores
		string jsonData = JsonConvert.SerializeObject(scores, Formatting.Indented);

		// save scores to file
		FileIO.SaveToFile("highscores.txt", jsonData);
	}

	public string ScoreList() {
		return ScoreList(10);
	}

	public string ScoreList(int num) {
		string output = "";
		output += " | Wave | Money | Points";
		output += "\n";
		output += "-------------------------";
		output += "\n";
		if (scores.Count == 0) {
			output += "        [ None ]        ";
		} else {
			scores.Sort();
			for (int i=0; i<num; i++) {
				if (i >= scores.Count) {
					break;
				}
				ScoreData score = scores[i];
				output += (i+1).ToString().PadLeft(2) + ": " + score.ToString() + "\n";
				/*output += i + ": ";
				output += "$" + score.wave;
				output += " | ";
				output += "$" + score.money;
				output += " | ";
				output += "$" + score.score;
				output += "\n";*/
			}
		}
		return output;
	}
}
