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
		CheckProgress();
		SaveProgress();
	}

	void CheckProgress()
	{
		ltl.LoadProgress();
		int rankPoints = 0;
		foreach(Tier tier in ltl.tiers) {
			foreach(Level level in tier.levels) {
				rankPoints += level.progress.rank;
			}
		}
		//TODO check if rankPoints have increased and display a message
		progress.rankPoints = rankPoints;
		foreach(Tier tier in ltl.tiers) {
			//TODO check for actual unlocking and display message
			if(tier.Unlocked)
				tier.progress.unlocked = true;
			//TODO check individual levels also
			foreach(Level level in tier.levels) {
				if(level.Unlocked)
					level.progress.unlocked = true;
			}
		}
		ltl.SaveProgress();
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

	public void ResetProgress()
	{
		progress.rankPoints = 0;
		ltl.ResetProgress();
	}

	[System.Serializable]
	public class ChallengeProgression
	{
		public int rankPoints;

		public override string ToString ()
		{
			return rankPoints.ToString();
		}
	}

}

