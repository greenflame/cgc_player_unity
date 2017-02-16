using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
	public float Step = 0.5f;
	public float Min = 0.5f;
	public float Max = 10f;

	void Start()
	{

	}

	void Update()
	{
		float speed = Time.timeScale;

		if (Input.GetKeyDown(KeyCode.Equals))
		{
			speed += Step;
		}

		if (Input.GetKeyDown(KeyCode.Minus))
		{
			speed -= Step;
		}

		speed = Mathf.Min(speed, Max);
		speed = Mathf.Max(speed, Min);

		Time.timeScale = speed;
	}
}
