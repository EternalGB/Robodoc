using UnityEngine;
using System.Collections;

public class Tutorial : Goal
{

	public override float EvaluateSuccess ()
	{
		return 0;
	}

	public override string FormatSuccess (float score)
	{
		return "";
	}

}

