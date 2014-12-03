using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
[ExecuteInEditMode]
public class ProbabilityTable : ScriptableObject
{
	
	public List<Object> items;
	public List<float> probabilities;

	public int Size
	{
		get {return items.Count;}
	}

	float maxProb;

	public ProbabilityTable()
	{
		items = new List<Object>();
		probabilities = new List<float>();
		maxProb = 0;
	}

	public void AddItem(Object newItem)
	{
		float n = items.Count;
		if(n > 0) {
			float smallestProb = GetSmallestProb(probabilities);
			float newSmallestProb = (n/(n+1))*smallestProb;
			for(int i = 0; i < probabilities.Count; i++) {
				probabilities[i] = (probabilities[i]/smallestProb)*newSmallestProb;
			}
			items.Add(newItem);
			probabilities.Add(newSmallestProb);
			maxProb = GetMaxProb();
		} else {
			items.Add(newItem);
			probabilities.Add(1);
			maxProb = 1;
		}
	}

	float GetMaxProb()
	{
		float best = float.MaxValue;
		foreach(float p in probabilities) {
			if(p > best)
				best = p;
		}
		return best;
	}

	public void SetProbability(int i, float newProb)
	{
		float difference = (probabilities[i] - newProb);
		if(difference < 0) {
			int numZeroes = 0;
			for(int j = 0; j < Size; j++) {
				if(probabilities[j] <= 0)
					numZeroes++;
			}
			difference /= Size-numZeroes-1;
			for(int j = 0; j < Size; j++)
				if(i != j && probabilities[j]+difference > 0)
					probabilities[j] += difference;

		} else {
			int numOnes = 0;
			for(int j = 0; j < Size; j++) {
				if(probabilities[j] == 0)
					numOnes++;
			}
			difference /= Size-numOnes-1;
			for(int j = 0; j < Size; j++)
				if(i != j && probabilities[j]+difference < 1)
					probabilities[j] += difference;
		}
		probabilities[i] = newProb;
		maxProb = GetMaxProb();
	}

	public void RemoveEntry(int i)
	{
		float prob = probabilities[i]/(Size-1);
		for(int j = 0; j < Size; j++)
			if(i != j)
				probabilities[j] += prob;
		probabilities.RemoveAt(i);
		items.RemoveAt (i);
	}

	static float GetSmallestProb(List<float> probs)
	{
		float best = float.MaxValue;
		foreach(float p in probs) {
			if(p < best)
				best = p;
		}
		return best;
	}


	public Object GetRandom()
	{
		float[] cumuProb = new float[Size];
		cumuProb[0] = probabilities[0];
		for(int i = 1; i < Size; i++) {
			cumuProb[i] = cumuProb[i-1]+probabilities[i];
		}

		float randProb = Random.value;
		int index = System.Array.BinarySearch<float>(cumuProb,randProb);
		//convert negative index
		if(index < 0) {
			index = Mathf.Abs(index+1);
		}

		return items[index];
	}
	

	public void ResetProbabilities()
	{
		float prob = 1f/Size;
		for(int i = 0; i < Size; i++) {
			probabilities[i] = prob;
		}
	}

	public bool Contains(Object item)
	{
		foreach(Object o in items) {
			if(o.Equals(item))
				return true;
		}
		return false;
	}

	public void Clear()
	{
		items = new List<Object>();
		probabilities = new List<float>();
		maxProb = 0;
	}



}

