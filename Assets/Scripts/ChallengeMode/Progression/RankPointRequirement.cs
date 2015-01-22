using UnityEngine;
using UnityEditor;
using System.Collections;

[System.Serializable]
public class RankPointRequirement : UnlockRequirement
{

	public int rankPointRequirement;

	public override bool RequirementMet ()
	{
		return ChallengeProgressionManager.Instance.progress.rankPoints >= rankPointRequirement;
	} 

	public override void DrawInInspector ()
	{
		rankPointRequirement = EditorGUILayout.IntField("Required Rank Points ",rankPointRequirement);
	}


}

