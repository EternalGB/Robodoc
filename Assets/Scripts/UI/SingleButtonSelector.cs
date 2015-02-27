using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(Button))]
public class SingleButtonSelector : MonoBehaviour
{

	int index;
	public List<string> texts;
	public Text displayText;

	public void SelectNext()
	{
		index = (index+1)%texts.Count;
		displayText.text = texts[index];
	}


	public void SetIndex(int index)
	{
		if(index >= 0 && index < texts.Count) {
			this.index = index;
			displayText.text = texts[index];
		}
	}

	public int GetIndex()
	{
		return index;
	}


}

