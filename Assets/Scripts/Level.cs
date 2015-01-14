using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Level : ScriptableObject
{

	public string displayName;
	public string sceneName;
	public Goal goal;
	public BallMachine ballMachinePrefab;
	

}

