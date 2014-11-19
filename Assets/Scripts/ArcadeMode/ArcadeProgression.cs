using UnityEngine;
using System.Collections.Generic;

public class ArcadeProgression 
{

	static string[] badBallNames = new string[]
	{
		"Ejector","Delinker","Infector"
	};

	public static List<string> GetUnlockedBadBalls()
	{
		List<string> balls;
		if(!Util.TryLoadFromPlayerPrefs<List<string>>("BBUnlocked",out balls)) {
			balls = new List<string>();
			balls.Add(badBallNames[0]);
			Util.SaveToPlayerPrefs<List<string>>("BBUnlocked",balls);
		}
		return balls;
	}

	public static void SetBadBallIndex(int newIndex)
	{
		int index;
		Util.TryLoadFromPlayerPrefs<int>("ArcadeBBIndex",out index);
		if(newIndex > index) {
			for(int i = index; i < newIndex && i < badBallNames.Length; i++) {
				AddBadBall(badBallNames[i]);
			}
		}
	}

	static void AddBadBall(string name)
	{
		List<string> balls = GetUnlockedBadBalls();
		balls.Add(name);
		Util.SaveToPlayerPrefs<List<string>>("BBUnlocked",balls);
	}

}

