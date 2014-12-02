using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class Unlockable : MonoBehaviour
{

	public Sprite unlockIcon;
	public string heading;
	public string unlockableName;

	public bool Unlocked
	{
		get
		{
			int unlocked = 0;
			Util.TryLoadFromPlayerPrefs<int>(unlockableName,out unlocked);
			return unlocked == 1;
		}
		private set
		{
			if(value)
				Util.SaveToPlayerPrefs<int>(unlockableName,1);
			else
				Util.SaveToPlayerPrefs<int>(unlockableName,0);
		}
	}

	public abstract bool ConditionMet();

	public void Unlock()
	{
		Unlocked = true;
		UnlockEffect();
	}

	public abstract void UnlockEffect();

	public void Reset()
	{
		PlayerPrefs.DeleteKey(unlockableName);
	}

}

