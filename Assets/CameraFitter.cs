using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFitter : MonoBehaviour {

	public float CameraCenterX = 10;
	public float CameraLeftBorder = -2;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		Vector3 center = Camera.main.WorldToScreenPoint(new Vector3(CameraCenterX, 0, 0));
		Vector3 left = Camera.main.WorldToScreenPoint(new Vector3(CameraLeftBorder, 0, 0));

		Camera.main.orthographicSize = Camera.main.orthographicSize / (Screen.width / 2) * (center - left).x;
	}
}