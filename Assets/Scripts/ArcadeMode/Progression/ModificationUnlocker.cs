using UnityEngine;
using System.Collections;

public class ModificationUnlocker : Unlockable
{

	public ProbabilityTable pt;
	public LevelModifier mod;
	public ArcadeStats.StatKeys stat;
	public float unlockRequirement;

	public override bool ConditionMet ()
	{
		return ArcadeStats.GetStat(stat) >= unlockRequirement;
	}
	
	public override void UnlockEffect ()
	{
		if(!pt.Contains(mod))
			pt.AddItem(mod);
	}

}

