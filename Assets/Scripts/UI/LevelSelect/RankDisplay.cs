using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class RankDisplay : MonoBehaviour
{

	public List<Image> stars;
	Color locked, unlocked;

	void OnEnable()
	{
		locked = Color.grey;
		unlocked = Color.white;
	}

	public virtual void SetRank(int rank)
	{
		if(stars != null) {
			for(int i = 0; i < stars.Count; i++) {
				Color currColor = stars[i].color;
				if(i < rank)
					currColor = unlocked;
				else
					currColor = locked;
				stars[i].color = currColor;
			}
		}
	}
		
}

