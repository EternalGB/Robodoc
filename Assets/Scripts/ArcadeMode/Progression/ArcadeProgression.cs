using UnityEngine;
using System.Collections.Generic;

public class ArcadeProgression 
{

	static Unlockable[] initUnlockables = new Unlockable[]
	{
		new BadBallUnlocker(2,8000), new BadBallUnlocker(3,10000), new BadBallUnlocker(4,12000), new BadBallUnlocker(5,16000), new BadBallUnlocker(6,20000),
		new ColorBallUnlocker(4,50000), new ColorBallUnlocker(5,100000)
	};

	static Unlockable[] unlockables;

	public static int BadBallIndex
	{
		get
		{
			int index;
			if(Util.TryLoadFromPlayerPrefs<int>("ArcadeBBIndex",out index))
				return index;
			else
				return 1;
		}
		set
		{
			Util.SaveToPlayerPrefs<int>("ArcadeBBIndex",value);
		}

	}

	public static int MaxColorBalls
	{
		get
		{
			int num;
			if(Util.TryLoadFromPlayerPrefs<int>("ArcadeMCB",out num))
				return num;
			else
				return 3;
		}
		set
		{
			Util.SaveToPlayerPrefs<int>("ArcadeMCB",value);
		}
	}
	
	public void UpdateProgression()
	{
		if(!Util.TryLoadFromPlayerPrefs<Unlockable[]>("ArcadeUnlocks",out unlockables))
			unlockables = initUnlockables;
		foreach(Unlockable unlock in unlockables) {
			if(!unlock.Unlocked && unlock.ConditionMet()) {
				unlock.Unlock();
			}
		}
		Util.SaveToPlayerPrefs("ArcadeUnlocks",unlockables);
	}

	public void Clear()
	{
		PlayerPrefs.DeleteKey("ArcadeUnlocks");
		PlayerPrefs.DeleteKey("ArcadeMCB");
		PlayerPrefs.DeleteKey("ArcadeBBIndex");
	}



}

