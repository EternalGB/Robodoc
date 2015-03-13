using UnityEngine;
using System.Collections.Generic;

public class AnimatedRankDisplay : MonoBehaviour
{

	public List<RankStar> stars;
	int rank;

	public void SetRank (int rank)
	{
		if(stars != null) {
			this.rank = rank;
			ProcessAnimation(0);
		}
	}

	void ProcessAnimation(int i)
	{
		if(i < stars.Count && i < rank) {
			Animator anim = stars[i].GetComponent<Animator>();
			anim.SetBool("activated",true);
			StartCoroutine(WaitForAnimation(stars[i],i));
		}
	}

	System.Collections.IEnumerator WaitForAnimation(RankStar star, int i)
	{
		while(!star.animationFinished) {
			yield return 0;
		}

		ProcessAnimation(i+1);
	}
}

