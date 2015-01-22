using UnityEngine;
using System.Collections;

[System.Serializable]
public class AlwaysUnlocked : UnlockRequirement
{

	public override bool RequirementMet ()
	{
		return true;
	} 

	public override void DrawInInspector ()
	{

	}

}

