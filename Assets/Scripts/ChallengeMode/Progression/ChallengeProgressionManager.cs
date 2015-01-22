using UnityEngine;
using System.Collections;

public class ChallengeProgressionManager : MonoBehaviour
{

	public LevelTiersList ltl;
	public ChallengeProgression progress;

	public static ChallengeProgressionManager Instance {get; private set;}

	void Awake()
	{
		if(Instance != null && Instance != this)
			Destroy(Instance);
		else
			Instance = this;
	}

	void Start()
	{
		LoadProgress();
	}

	public void LoadProgress()
	{
		ChallengeProgression loadedProgress;
		if(Util.TryLoadFromPlayerPrefs<ChallengeProgression>("ChallengeProgression", out loadedProgress))
			progress = loadedProgress;
		else {
			progress = new ChallengeProgression{rankPoints = 0};
		}
	}

	public void SaveProgress()
	{
		if(progress == null)
			LoadProgress();
		else {
			Util.SaveToPlayerPrefs<ChallengeProgression>("ChallengeProgression", progress);
		}
	}

	void OnDisable()
	{
		SaveProgress();
	}

	public class ChallengeProgression
	{
		public int rankPoints;
	}

}

