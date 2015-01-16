using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class Goal : ScriptableObject
{

	public string displayName;
	public float initTime;
	protected float timeRemaining;
	public float displayTime;
	protected float lerpTimer;
	protected float lerpSpeed = 0.25f;

	public virtual bool Completed()
	{
		return timeRemaining <= 0;
	}

	public abstract float EvaluateSuccess();


	public abstract string FormatSuccess(float score);

	protected void OnEnable()
	{
		ResetGoal();

	}

	public void UpdateDisplayTime()
	{
		//kinda doing weird stuff here because UpdateTime will always be called every update by GameGUI
		displayTime = Mathf.Lerp (displayTime,timeRemaining,lerpTimer);
		lerpTimer = Mathf.Clamp (lerpTimer + lerpSpeed*Time.deltaTime,0,1f);
		UpdateTime();
	}

	protected virtual void UpdateTime()
	{
		timeRemaining -= Time.deltaTime;
	}

	public virtual void ResetGoal()
	{
		lerpTimer = 0;
		timeRemaining = initTime;
	}

}

