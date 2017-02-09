﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

	public float Height = 1;
	public float TotalTime = 1;

	private float TimeRest;

	// Use this for initialization
	void Start () {
		TimeRest = TotalTime;
		transform.Translate(Vector3.up * Height);
	}
	
	// Update is called once per frame
	void Update () {
		if (TimeRest > 0)
		{
			transform.Translate(Vector3.down * Height / TotalTime * Time.deltaTime);
			TimeRest -= Time.deltaTime;
		}
		else
		{
			GameObject.Destroy(this);
		}
	}
}
