using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ArcadeStatsViewer : MonoBehaviour
{

	public List<string> names;
	public List<float> values;

	void Start()
	{

		#if UNITY_EDITOR
		names = new List<string>();
		values = new List<float>();

		System.Array keyValues = System.Enum.GetValues(typeof(ArcadeStats.StatKeys));
		for(int i = 0; i < keyValues.Length; i++) {
			ArcadeStats.StatKeys key = (ArcadeStats.StatKeys)keyValues.GetValue(i);
			names.Add(System.Enum.GetName(typeof(ArcadeStats.StatKeys),i));
			values.Add(ArcadeStats.GetStat(key));
		}
		#endif

	}

}

