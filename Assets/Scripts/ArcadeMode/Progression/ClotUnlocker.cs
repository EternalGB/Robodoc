using UnityEngine;
using System.Collections;

public class ClotUnlocker : Unlockable
{

	public ArcadeManager manager;

	public override void UnlockEffect ()
	{
		manager.SetClotSpawnInterval(40);
	}

}

