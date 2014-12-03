using UnityEngine;
using System.Collections.Generic;

public class ArcadeManager : MonoBehaviour
{

	public BallMachine bm;
	public ArcadeProgression progression;

	public Material baseMat;
	public List<GameObject> goodBallPrefabs;
	public List<GameObject> badBalls;
	public int initBadBalls = 2;

	public ProbabilityTable modifications;



	float scoreTotal;
	public float initMilestone;
	float milestone;
	int milestoneIndex = 0;
	public float milestoneIncreaseRate;



	void Awake()
	{
		modifications.Clear();
		progression.ProcessUnlocked();
		while(goodBallPrefabs.Count > 0) {
			AddColorBall();
		}
		for(int i = 0; i < initBadBalls; i++) {
			AddBadBall();
		}
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
		if(scoreTotal >= milestone) {
			if(modifications != null && modifications.Size > 0)
				((LevelModifier)modifications.GetRandom()).DoModification();
			if(badBalls.Count > 0)
				AddBadBall();
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
		int badBallIndex = Random.Range(0,badBalls.Count);
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

