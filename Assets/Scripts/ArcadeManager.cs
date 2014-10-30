using UnityEngine;
using System.Collections.Generic;

public class ArcadeManager : MonoBehaviour
{

	public BallMachine bm;
	public int maxColors;
	public int numInitColors;
	public Material baseMat;
	public GameObject goodBallPrefab;
	public List<GameObject> badBalls;

	Palette colors;
	Material[] ballMats;
	int matIndex = 0;

	void Awake()
	{
		colors =  new Palette(maxColors,0.99f,0.99f);
		ballMats = new Material[maxColors];
		for(int i = 0; i < ballMats.Length; i++) {
			ballMats[i] = new Material(baseMat);
			ballMats[i].color = colors.GetColor(i);
		}
		for(matIndex = 0; matIndex < numInitColors; matIndex++) {
			GameObject newBall = (GameObject)GameObject.Instantiate(goodBallPrefab);
			newBall.SetActive(false);
			newBall.renderer.material = ballMats[matIndex];
			bm.AddGoodBall(newBall);
		}
		int badBallIndex = Random.Range(0,badBalls.Count);
		bm.AddBadBall(badBalls[badBallIndex]);
		badBalls.RemoveAt(badBallIndex);
	}

}

