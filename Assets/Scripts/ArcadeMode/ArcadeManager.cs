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

	Palette colors;
	Material[] ballMats;
	int matIndex = 0;

	float scoreTotal;
	public float initMilestone;
	float milestone;
	int milestoneIndex = 0;
	public float milestoneIncreaseRate;

	void Awake()
	{
		colors =  new Palette(maxColors,0.99f,1f,0.05f);
		ballMats = new Material[maxColors];
		for(int i = 0; i < ballMats.Length; i++) {
			ballMats[i] = new Material(baseMat);
			ballMats[i].color = colors.GetColor(i);
		}
		for(matIndex = 0; matIndex < numInitColors; matIndex++) {
			AddColorBall(matIndex);
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
		if(scoreTotal >= milestone) {
			//do some stuff
			((LevelModifier)modifications.GetRandom()).DoModification();
			if(badBalls.Count > 0)
				AddBadBall();
			//+1 because of bonus ball
			if(bm.NumGoodBalls() < maxColors+1) {
				AddColorBall(matIndex);
				matIndex++;
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
		int badBallIndex = Random.Range(0,badBalls.Count);
		bm.AddBadBall(badBalls[badBallIndex]);
		badBalls.RemoveAt(badBallIndex);
	}

	void AddColorBall(int matIndex)
	{
		GameObject newBall = (GameObject)GameObject.Instantiate(Util.GetRandomElement(goodBallPrefabs));
		newBall.SetActive(false);
		newBall.GetComponent<SpriteRenderer>().material = ballMats[matIndex];
		bm.AddGoodBall(newBall);
	}
}

