using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class Unlockable
{

	public bool Unlocked
	{
		get; private set;
	}

	public abstract bool ConditionMet();

	public void Unlock()
	{
		Unlocked = true;
		UnlockEffect();
	}

	protected abstract void UnlockEffect();

}

