using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class RankDisplay : MonoBehaviour
{

	public List<Image> stars;

	public virtual void SetRank(int rank)
	{
		if(stars != null) {
			for(int i = 0; i < stars.Count; i++) {
				Color currColor = stars[i].color;
				if(i < rank)
					currColor.a = 1;
				else
					currColor.a = 0;
				stars[i].color = currColor;
			}
		}
	}
		
}

