using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFitter : MonoBehaviour {

	public float CameraCenterX = 10;
	public float CameraLeftBorder = -2;
	public float Epsilon = 0.1f;

	void Start () {

	}
	
	void Update () {

		Vector3 center = Camera.main.WorldToScreenPoint(new Vector3(CameraCenterX, 0, 0));
		Vector3 left = Camera.main.WorldToScreenPoint(new Vector3(CameraLeftBorder, 0, 0));

		float k = (center - left).x / (Screen.width / 2);

		if (Mathf.Abs(k - 1f) > Epsilon)
		{
			Camera.main.orthographicSize *= k;
		}
	}
}