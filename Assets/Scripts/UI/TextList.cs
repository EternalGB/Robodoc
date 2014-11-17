using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TextList : MonoBehaviour
{

	public int index;
	public List<string> texts;
	public Text displayText;



	public void DisplayText(int index)
	{
		if(index >= 0 && index < texts.Count) {
			Debug.Log ("Changing index to " + index + ", text = " + texts[index]);
			this.index = index;
			displayText.text = texts[index];
		}
	}

	public void NextText()
	{
		index = Mathf.Clamp(index+1,0,texts.Count-1);
		displayText.text = texts[index];
	}

	public void PreviousText()
	{
		index = Mathf.Clamp(index-1,0,texts.Count-1);
		displayText.text = texts[index];
	}
}

