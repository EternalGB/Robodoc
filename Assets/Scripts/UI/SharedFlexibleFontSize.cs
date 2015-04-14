using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(Text))]
[ExecuteInEditMode]
public class SharedFlexibleFontSize : MonoBehaviour
{

	public List<Text> otherTexts;
	Text thisText;

	void OnEnable()
	{
		thisText = GetComponent<Text>();
	}

	void Update ()
	{
		if(otherTexts != null) {
			int min = int.MaxValue;
			foreach(Text text in otherTexts) {
				if(text.resizeTextForBestFit)
					min = Mathf.Min (text.cachedTextGenerator.fontSizeUsedForBestFit,min);
				else
					min = Mathf.Min (text.fontSize, min);
			}
			thisText.fontSize = min;
		}
	}
}

