using System;
using System.Collections.Generic;

public class ScoreData : IEquatable<ScoreData>, IComparable<ScoreData> {
	public int score = 0;
	public int money = 0;
	public int wave = 0;

	public override string ToString() {
		string waveString = wave.ToString().PadLeft(3);
		string moneyString = ("$" + money).PadLeft(6);
		string scoreString = score.ToString().PadLeft(6);
		return (waveString + " | " + moneyString + " | " + scoreString);
	}

	public bool Equals(ScoreData otherScoreData) {
		if (otherScoreData == null) {
			return false;
		} else {
			return this.score.Equals(otherScoreData.score);
		}
	}

	public int CompareTo(ScoreData otherScoreData) {
		if (otherScoreData == null) {
			return 1;
		} else {
			//return this.score.CompareTo(otherScoreData.score); // ascending - best scores last
			return otherScoreData.score.CompareTo(this.score); // descending - best scores first
		}
	}
}