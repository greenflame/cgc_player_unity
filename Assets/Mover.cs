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

    public GameObject ObjectTarget;
    public float PositionTarget;

	public TargetTypeE TargetType;
	public TrajectoryTypeE TrajectoryType;

	void Start () {

    }

	public void InitiateTargetMotion(Vector3 startPos, GameObject target, float speed, TrajectoryTypeE trajectory)
	{
		StartPos = startPos;
		ObjectTarget = target;
		Speed = speed;
		TrajectoryType = trajectory;

		TargetType = TargetTypeE.Object;
	}

	public void InitiatePosMotion(Vector3 startPos, float target, float speed, TrajectoryTypeE trajectory)
	{
		StartPos = startPos;
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


		float deltaPos = targetPos - transform.position.x;

        if (deltaPos == 0)
        {
            return;
        }

		if (TrajectoryType == TrajectoryTypeE.Parabolic)
		{
			float flightPeak = (StartPos.x + targetPos) / 2;
			float flightLength = Mathf.Abs(StartPos.x - targetPos);
			float maxHeight = Mathf.Sqrt(flightLength) / 3;
			pos.y = StartPos.y - (Mathf.Pow((pos.x - flightPeak) / flightLength * 2, 2) - 1) * maxHeight;
		}

		pos.x = pos.x + deltaPos / Mathf.Abs(deltaPos) * Speed * Time.deltaTime;

        transform.position = pos;
	}
}
