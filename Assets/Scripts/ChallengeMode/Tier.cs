using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Tier : ScriptableObject
{
	public List<Level> levels;
	public bool unlocked;

}



