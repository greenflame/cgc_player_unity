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
	public float StartPos;

    public GameObject ObjectTarget;
    public float PositionTarget;

	public TargetTypeE TargetType;
	public TrajectoryTypeE TrajectoryType;

	void Start () {

    }

	public void InitiateTargetMotion(float startPos, GameObject target, float speed, TrajectoryTypeE trajectory)
	{
		StartPos = startPos;
		ObjectTarget = target;
		Speed = speed;
		TrajectoryType = trajectory;

		TargetType = TargetTypeE.Object;
	}

	public void InitiatePosMotion(float startPos, float target, float speed, TrajectoryTypeE trajectory)
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
			float flightPeak = (StartPos + targetPos) / 2;
			float flightLength = Mathf.Abs(StartPos - targetPos);
			float maxHeight = Mathf.Sqrt(flightLength) / 3;
			pos.y = - (Mathf.Pow((pos.x - flightPeak) / flightLength * 2, 2) - 1) * maxHeight;
		}

		pos.x = pos.x + deltaPos / Mathf.Abs(deltaPos) * Speed * Time.deltaTime;

        transform.position = pos;
	}
}
