using UnityEngine;
using System.Collections;

public abstract class Goal : ScriptableObject
{

	public string displayName;

	public abstract bool Completed();

	public abstract float EvaluateSuccess();

	public abstract void DisplayProgress(GUIStyle textStyle, GUIStyle predictionStyle);

	public abstract void DisplaySuccess(GUIStyle textStyle);

	public abstract string FormatSuccess(float score);

}

