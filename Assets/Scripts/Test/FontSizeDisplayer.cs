using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
[ExecuteInEditMode]
public class FontSizeDisplayer : MonoBehaviour
{

	public float fontSize;

	void Update()
	{
		fontSize = GetComponent<Text>().cachedTextGenerator.fontSizeUsedForBestFit;
	}
}

