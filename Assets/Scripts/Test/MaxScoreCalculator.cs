using UnityEngine;
using System.Collections;

public class MaxScoreCalculator : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		int comboMulti = 11;
		int seconds = 300;
		Debug.Log ("Easy = " + comboMulti*CalculateAverageScore(0.6f,0.12f,				0.2f,seconds,1000));
		Debug.Log ("Easy Obstacle = " + comboMulti*CalculateAverageScore(0.6f,0.12f,	0.4f,seconds,1000));
		Debug.Log ("Medium = " + comboMulti*CalculateAverageScore(0.83f,0.16f,			0.6f,seconds,1000));
		Debug.Log ("Medium Obstacle = "+comboMulti*CalculateAverageScore(0.83f,0.16f,	0.85f,seconds,1000));
		Debug.Log ("Hard = " + comboMulti*CalculateAverageScore(1.16f,0.23f,			0.85f,seconds,1000));
		Debug.Log ("Hard Obstacle = " + comboMulti*CalculateAverageScore(1.16f,0.23f,	0.9f,seconds,1000));
		Debug.Log ("Very Hard = " + comboMulti*CalculateAverageScore(1.5f,0.3f,			0.9f,seconds,1000));
		Debug.Log ("Very Hard Obstacle = " + comboMulti*CalculateAverageScore(1.5f,0.3f,0.95f,seconds,1000));
		Debug.Log ("Random Levels = " + comboMulti*CalculateAverageScore(1.16f,0.23f,	0.95f,seconds,1000));
	}
	
	public static int CalculateMaxScore(float d, float b, int seconds)
	{
		int numD = (int)(seconds*d);
		int numB = (int)(seconds*b);
		int ratioDB = numD/numB;
		return CalcSum(numD, ratioDB);
	}

	public static float CalculateAverageScore(float d, float b, float missChance, int seconds, int numIter)
	{
		int averageChains = 5;
		d /= averageChains;
		b /= averageChains;
		int numD = (int)(seconds*d);
		int numB = (int)(seconds*b);
		int ratioDB = numD/numB;
		return averageChains*CalcAverageMax(numD,ratioDB,missChance,numIter);
	}

	public static float CalcAverageMax(int numD, int ratio, float missChance, int numIter)
	{
		float total = 0;
		for(int i = 0; i < numIter; i++) {
			total += CalcSum(numD,ratio,missChance);
		}

		return total/numIter;
	}

	public static int CalcSum(int numD, int ratio, float missChance)
	{
		int sum = 0;
		for(int i = 1; i <= numD; i++) {
			if(Random.value >= missChance) {
				if(i%ratio == 0)
					sum += i*2;
				else
					sum += i;
			}

		}
		return sum;
	}

	public static int CalcSum(int numD, int ratio)
	{
		int sum = 0;
		for(int i = 1; i <= numD; i++) {
			if(i%ratio == 0)
				sum += i*2;
			else
				sum += i;
		}
		return sum;
	}

}

