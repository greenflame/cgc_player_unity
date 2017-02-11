using UnityEngine;
using System.Collections;
using System.IO;

public class Mover : MonoBehaviour {

	public enum TargetTypeE
	{
		Object,
		Position
	}

	public enum TrajectoryTypeE
	{
		Linear,
		Parabolic
	}
		
    public float Speed;
	public Vector3 StartPos;
	public float StartTime;

    public GameObject ObjectTarget;
    public float PositionTarget;

	public TargetTypeE TargetType;
	public TrajectoryTypeE TrajectoryType;

	private MainController MainController;

	void Start () {
		MainController = GameObject.Find("MainController").GetComponent<MainController>();
    }

	public void InitiateTargetMotion(Vector3 startPos, GameObject target, float speed, TrajectoryTypeE trajectory)
	{
		StartPos = startPos;
		StartTime = GameObject.Find("MainController").GetComponent<MainController>().GloablTime;
		ObjectTarget = target;
		Speed = speed;
		TrajectoryType = trajectory;

		TargetType = TargetTypeE.Object;
	}

	public void InitiatePosMotion(Vector3 startPos, float target, float speed, TrajectoryTypeE trajectory)
	{
		StartPos = startPos;
		StartTime = GameObject.Find("MainController").GetComponent<MainController>().GloablTime;
		PositionTarget = target;
		Speed = speed;
		TrajectoryType = trajectory;

		TargetType = TargetTypeE.Position;
	}
	
	void Update () {
		Vector3 pos = transform.position;
		float targetPos = 0;

		switch (TargetType)
		{
			case TargetTypeE.Object:
				targetPos = ObjectTarget.transform.position.x;
				break;
			case TargetTypeE.Position:
				targetPos = PositionTarget;
				break;
		}
				
		if (TrajectoryType == TrajectoryTypeE.Parabolic)
		{
			float flightPeak = (StartPos.x + targetPos) / 2;
			float flightLength = Mathf.Abs(StartPos.x - targetPos);
			float maxHeight = Mathf.Sqrt(flightLength) / 3;
			pos.y = StartPos.y - (Mathf.Pow((pos.x - flightPeak) / flightLength * 2, 2) - 1) * maxHeight;
		}

		pos.x = StartPos.x + Mathf.Sign(targetPos - StartPos.x) * Speed * (MainController.GloablTime - StartTime);

        transform.position = pos;
	}
}
