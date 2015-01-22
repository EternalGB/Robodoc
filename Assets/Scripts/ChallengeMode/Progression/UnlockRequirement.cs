using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class UnlockRequirement : ScriptableObject
{

	void OnEnable()
	{
		//super important, stops Unity from GCing the object
		hideFlags = HideFlags.HideAndDontSave;
		DontDestroyOnLoad(this);
	}

	public abstract bool RequirementMet();

	public abstract void DrawInInspector();

}

