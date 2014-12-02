using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ArcadeProgression : MonoBehaviour
{

	public GameObject unlockMessageBox;
	public Text unlockableHeadingText;
	public Text unlockableNameText;
	public Image unlockableIconImage;
	public List<Unlockable> unlockables;
	bool messageConfirmed = false;

	public System.Collections.IEnumerator UpdateProgression()
	{
		foreach(Unlockable unlock in unlockables) {
			if(!unlock.Unlocked && unlock.ConditionMet()) {
				unlock.Unlock();
				yield return StartCoroutine(DisplayUnlockMessage(unlock));
			}
		}
	}

	public void ProcessUnlocked()
	{
		if(PlayerPrefs.GetInt("ArcadeProgressionCleared",0) == 1) {
			foreach(Unlockable unlock in unlockables) {
				unlock.Reset();
			}
			PlayerPrefs.SetInt("ArcadeProgressionCleared",0);
		}
		foreach(Unlockable unlock in unlockables) {
			if(unlock.Unlocked) {
				unlock.UnlockEffect();
			}
		}
	}

	System.Collections.IEnumerator DisplayUnlockMessage(Unlockable unlock)
	{
		while(!messageConfirmed) {
			unlockMessageBox.SetActive(true);
			unlockableHeadingText.text = unlock.heading;
			unlockableNameText.text = unlock.unlockableName;
			unlockableIconImage.sprite = unlock.unlockIcon;
			yield return 0;
		}

		unlockMessageBox.SetActive(false);
		unlockableNameText.text = "";
		messageConfirmed = false;
	}
	
	public void SetMessageConfirmed()
	{
		messageConfirmed = true;
	}

	public static void Clear()
	{
		PlayerPrefs.SetInt("ArcadeProgressionCleared",1);
	}



}

