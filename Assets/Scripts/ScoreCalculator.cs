using UnityEngine;
using System.Collections.Generic;

public class ScoreCalculator : MonoBehaviour
{

	public static ScoreCalculator Instance
	{
		get; private set;
	}
	
	void Awake()
	{
		if(Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		
		Instance = this;


	}

	GameGUI gui;
	PlayerBall player;

	void Start()
	{
		gui = GameObject.Find("GameGUI").GetComponent<GameGUI>();
		player = GameObject.Find ("PlayerBall").GetComponent<PlayerBall>();
	}

	public GameObject scoringBallPrefab;
	public GameObject scoringBallChildPrefab;

	public float maxChainMultiplier = 3;


	public int maxComboMultiplier = 11;
	public int comboMulti = 1;
	public float comboTimeout;
	
	public float score = 0;
	public float nextScore = 0;
	public float biggestScore = 0;
	public int longestChain = 0;
	public int nextLongestChain = 0;
	
	float lerpTimer;
	float lerpSpeed = 0.25f;
	public float displayScore = 0;
	public float displayNextScore = 0;
	public float displayBiggestScore = 0;
	public float displayLongestChain = 0;
	public float displayNextLongestChain = 0;

	public void ScoreStructure()
	{
		int maxChain = 0;
		float amount = GetPoints(player.transform,out maxChain);
		biggestScore = Mathf.Max(biggestScore,amount);
		longestChain = Mathf.Max(longestChain,maxChain);
		score += amount;
		lerpTimer = 0;
		nextScore = 0;
		CancelInvoke("ResetCombo");
		CreateScoringBalls(player.transform,null);
		Util.DestroyChildren(player.transform);
		ResetCombo();
		GameObject.FindWithTag("BallMachine").SendMessage("UpdateDifficulty",amount);
	}

	public void SetScorePrediction()
	{
		int maxChain;
		nextScore = GetPoints(player.transform,out maxChain);
		nextLongestChain = Mathf.Max(nextLongestChain,maxChain);
	}

	void Update()
	{
		displayScore = Mathf.Floor(Mathf.Lerp (displayScore,score,lerpTimer));
		displayNextScore = Mathf.Floor(Mathf.Lerp (displayNextScore,nextScore,lerpTimer));
		displayBiggestScore = Mathf.Floor(Mathf.Lerp (displayBiggestScore,biggestScore,lerpTimer));

		lerpTimer = Mathf.Clamp (lerpTimer + lerpSpeed*Time.deltaTime,0,1f);
	}

	float GetPoints(Transform transform, out int maxDepth)
	{
		float score = ScoreChildren(transform, out maxDepth);
		score += maxDepth*maxChainMultiplier;
		return comboMulti*score;
	}
	
	float ScoreChildren(Transform t, out int maxChain)
	{
		float points = 0;
		int depth = 0;
		foreach(Transform child in t)
			points += ScoreChild(child,1, ref depth);
		maxChain = depth;
		return points;
	}
	
	float ScoreChild(Transform child, int depth, ref int maxChain)
	{
		if(depth > maxChain)
			maxChain = depth;
		AttachedBall ab = child.GetComponent<AttachedBall>();
		float points = depth*ab.pointValue;
		foreach(Transform c in child) {
			//if a connecting ball is golden or the same colour then continue the chain
			if(ComboValid(child,c)) {
				points += ScoreChild(c, depth+1, ref maxChain);
			} else {
				points += ScoreChild(c, 1, ref maxChain);
			}
		}
		//Debug.Log (child.name + child.GetInstanceID() + " scoring for " + points);
		return points;
	}
	
	int GetMaxChainLength(Transform t, int depth)
	{
		int maxDepth = depth;
		foreach(Transform child in t) {
			int newDepth = 0;
			if(ComboValid(t,child))
				newDepth = GetMaxChainLength(child,depth+1);
			else
				newDepth = GetMaxChainLength(child,1);
			if(newDepth > maxDepth)
				maxDepth = newDepth;
		}
		return maxDepth;
	}
	
	bool ComboValid(Transform parent, Transform child)
	{
		return child.renderer.sharedMaterial.name != "Infected" && parent.renderer.sharedMaterial.name != "Infected" &&
			(child.renderer.sharedMaterial.name == "AnyColour" || parent.renderer.sharedMaterial.name == "AnyColour" ||
			 child.renderer.sharedMaterial.Equals(parent.renderer.sharedMaterial));
	}
	
	public void ComboUp()
	{
		if(comboMulti < maxComboMultiplier)
			comboMulti++;
		CancelInvoke("ResetCombo");
		Invoke("ResetCombo",comboTimeout);
	}
	
	void ResetCombo()
	{
		comboMulti = 1;
		SetScorePrediction();
	}

	bool ScoringBallComboValid(Transform parent, Transform child)
	{
		if(parent.GetComponent<PlayerBall>())
			return false;
		bool infected = (child.renderer.sharedMaterial.name == "Infected" || parent.renderer.sharedMaterial.name == "Infected");
		if(infected)
			return (child.renderer.sharedMaterial.name == "Infected" && parent.renderer.sharedMaterial.name == "Infected");
		else
			return (child.renderer.sharedMaterial.name == "AnyColour" || parent.renderer.sharedMaterial.name == "AnyColour" ||
				 child.renderer.sharedMaterial.Equals(parent.renderer.sharedMaterial));
	}

	void CreateScoringBalls(Transform root, Transform lastScoringParent)
	{
		foreach(Transform child in root) {
			GameObject ball;
			if(ScoringBallComboValid(root,child)) {
				ball = PoolManager.Instance.GetPoolByRepresentative(scoringBallChildPrefab).GetPooled();
				ball.transform.localScale = child.transform.lossyScale;
				ball.transform.parent = lastScoringParent;
			} else {
				ball = PoolManager.Instance.GetPoolByRepresentative(scoringBallPrefab).GetPooled();
				ScoringBall sb = ball.GetComponent<ScoringBall>();
				sb.lifeTime = Random.Range(1f,2f);
				sb.speed = Random.Range(10f,20f);
				sb.rotSpeed = Random.Range(180f,360f);
				sb.travelDir = Quaternion.AngleAxis(Random.Range (-45f,45f),Vector3.forward)
					*(ball.transform.position - root.transform.position).normalized;
				ball.transform.localScale = child.transform.lossyScale;
			}
			ball.transform.position = child.position;
			ball.transform.rotation = child.rotation;
			ball.renderer.sharedMaterial = child.renderer.sharedMaterial;
			ball.particleSystem.startColor = child.renderer.sharedMaterial.color;

			ball.SetActive(true);
			ball.SendMessage("Arm",SendMessageOptions.DontRequireReceiver);
			CreateScoringBalls(child, ball.transform);
		}
	}

	Vector3 ScoringBallPosAverage(List<ScoringBall> balls) 
	{
		Vector3 total = Vector3.zero;
		foreach(ScoringBall ball in balls)
			total += ball.transform.position;
		total /= balls.Count;
		return total;
	}

}

