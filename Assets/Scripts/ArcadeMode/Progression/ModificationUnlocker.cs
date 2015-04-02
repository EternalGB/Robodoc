using UnityEngine;
using System.Collections;

public class ModificationUnlocker : Unlockable
{

	public ProbabilityTable pt;
	public LevelModifier mod;

	
	public override void UnlockEffect ()
	{
		if(!pt.Contains(mod))
			pt.AddItem(mod);
	}

}

