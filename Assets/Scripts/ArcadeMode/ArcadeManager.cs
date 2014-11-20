using UnityEngine;
using System.Collections.Generic;

public class ArcadeManager : MonoBehaviour
{

	public BallMachine bm;
	public int maxColors;
	public int numInitColors;
	public Material baseMat;
	public List<GameObject> goodBallPrefabs;
	public List<GameObject> badBalls;

	public ProbabilityTable modifications;



	float scoreTotal;
	public float initMilestone;
	float milestone;
	int milestoneIndex = 0;
	public float milestoneIncreaseRate;

	void Awake()
	{
		for(int i = 0; i < numInitColors; i++) {
			AddColorBall();
		}
		AddBadBall();
	}

	void Start()
	{
		ScoreCalculator.PlayerScored += UpdateDifficulty;
		milestone = GenMileStone(initMilestone,milestoneIncreaseRate,milestoneIndex);
		milestoneIndex++;
	}

	void UpdateDifficulty(float scoreIncrease)
	{
		scoreTotal += scoreIncrease;
		if(modifications != null && modifications.Size > 0 && scoreTotal >= milestone) {
			//do some stuff
			((LevelModifier)modifications.GetRandom()).DoModification();
			if(badBalls.Count > 0)
				AddBadBall();
			//+1 because of bonus ball
			if(bm.NumGoodBalls() < maxColors+1) {
				AddColorBall();
			}


			milestone = GenMileStone(initMilestone,milestoneIncreaseRate,milestoneIndex);
			milestoneIndex++;
		}
	}

	float GenMileStone(float initMilestone, float incrSpeed, int index)
	{
		return initMilestone*Mathf.Exp(incrSpeed*index);
	}

	void AddBadBall()
	{
		int badBallIndex = 0;//Random.Range(0,badBalls.Count);
		bm.AddBadBall(badBalls[badBallIndex]);
		badBalls.RemoveAt(badBallIndex);
	}

	void AddColorBall()
	{
		int ballIndex = Random.Range (0,goodBallPrefabs.Count);
		bm.AddGoodBall(goodBallPrefabs[ballIndex]);
		goodBallPrefabs.RemoveAt(ballIndex);
	}
}

