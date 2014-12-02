using UnityEngine;
using System.Collections;

public abstract class LevelModifier : ScriptableObject
{

	public abstract void DoModification();

	public override bool Equals (object o)
	{
		return o.GetType().Equals(this.GetType());
	}



}

