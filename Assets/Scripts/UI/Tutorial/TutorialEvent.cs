using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class TutorialEvent : MonoBehaviour
{

	public int priority;

	void Start()
	{
		InitEvent();
	}

	protected abstract void InitEvent();

	public abstract void Activate();

	public abstract void Deactivate();

	public abstract bool Completed();


}

