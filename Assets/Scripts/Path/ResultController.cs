using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ResultController : MonoBehaviour 
{
	[SerializeField] private Text resultText = null;
	//[SerializeField] private CarCameras carCamera = null;
	[SerializeField] private GameObject finishWindow = null;
	[SerializeField] private float timeToShowFinishWindow = 2;
	public int Laps = 2;
	public int[] Results;
	private int mesto = 4;
	private int characterCurrentWaypoint = 0;
	private bool isFinish = false;

	public bool IsFinish
	{
		get {return isFinish;}
		set 
		{
			isFinish = value;
			resultText.text = "";
			//carCamera.target = null;
		}
	}

	private void Start()
	{
		WayPoint[] points = FindObjectsOfType<WayPoint>();
		Array.Resize(ref Results, points.Length * Laps);
		finishWindow.SetActive(false);
	}

	public int CharacterCurrentWaypoint
	{
		get { return characterCurrentWaypoint; }
		set
		{
			characterCurrentWaypoint = value;
			if (!IsFinish)
			{
				mesto = Results[characterCurrentWaypoint] + 1;
				resultText.text = mesto.ToString("f0") + "/5";
			}
		}
	}

	public void CheckResults()
	{
		if (Results[Mathf.Min(characterCurrentWaypoint + 1, Results.Length - 1)] + 1 > mesto)
		{
			int mesto2 = Results[Mathf.Min(characterCurrentWaypoint + 1, Results.Length - 1)] + 1;
			if (mesto2 > mesto && !IsFinish)
			{
				resultText.text = mesto2.ToString("f0") + "/5";
			}
		}
	}

	public void Finish()
	{
		Invoke ("ShowFinishWindow",timeToShowFinishWindow);
	}

	private void ShowFinishWindow()
	{
		finishWindow.SetActive (true);
	}
}
