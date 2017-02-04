using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

public class MainController : MonoBehaviour
{

	public float GloablTime;
	private string[] Commands;
	private int Pointer;

	private Dictionary<string, GameObject> Objects;

	void Start()
	{
		Objects = new Dictionary<string, GameObject>();

		Commands = File.ReadAllLines("/Users/alexander/cgc_compiler/cgc_compiler/bin/Debug/game_log.txt");
		Pointer = 0;
		GloablTime = 0;
	}

	private IEnumerator DeleteCoro(GameObject target, float time)
	{
		yield return new WaitForSeconds(time);
		GameObject.Destroy(target);
	}

	private T GetOrAddComponent<T>(GameObject obj) where T : Component
	{
		T component = obj.GetComponent<T>();

		if (component == null)
		{
			component = obj.AddComponent<T>();
		}

		return component;
	}
		
	private void SetAnimation(GameObject obj, string animationName)
	{
		string animationFullName = obj.GetComponent<Badge>().Name + animationName;
		obj.GetComponent<Animator>().Play(animationFullName, 0, 0);
	}
		
	void Update()
	{
		while (Pointer < Commands.Length && float.Parse(Commands[Pointer].Split(' ')[1]) < GloablTime)
		{
			string[] args = Commands[Pointer].Split(' ');
			Debug.Log(Commands[Pointer]);
			Pointer++;

			string action = args[0];
			string id = args[2];

			args = args.Skip(3).ToArray();

			if (action == "CREATE")
			{
				string name = args[0];
				Badge.Player owner = (Badge.Player)Enum.Parse(typeof(Badge.Player), args[1]);

				GameObject tmp = (GameObject)Instantiate(Resources.Load(name));
				Objects.Add(id, tmp);

				Badge crystal = GetOrAddComponent<Badge>(tmp);

				crystal.Name = name;
				crystal.Owner = owner;
			}

			GameObject obj = Objects[id];

			if (action == "DESTROY")
			{
				StartCoroutine(DeleteCoro(obj, 0));
			}

			if (action == "DESTROY_DELAYED")
			{
				float interval = float.Parse(args[0]);

				StartCoroutine(DeleteCoro(obj, interval));
			}

			if (action == "SET_HEALTH")
			{
				int currentHealth = (int)Math.Round(float.Parse(args[0]));
				int maxHealth = (int)Math.Round(float.Parse(args[1]));

				Health health = GetOrAddComponent<Health>(obj);

				health.MaxHealth = maxHealth;
				health.CurrentHealth = currentHealth;
			}

			if (action == "SET_POSITION")
			{
				float pos = float.Parse(args[0]);

				Vector3 tmp = obj.transform.position;
				tmp.x = pos;
				obj.transform.position = tmp;
			}
				
			if (action == "SET_DIRECTION_TARGET")
			{
				GameObject target = Objects[args[0]];

				SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
				spriteRenderer.flipX = target.transform.position.x < obj.transform.position.x;
			}

			if (action == "SET_DIRECTION_POS")
			{
				float target = float.Parse(args[0]);

				SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
				spriteRenderer.flipX = target < obj.transform.position.x;
			}

			if (action == "MOTION_RESET")
			{
				GameObject.Destroy(obj.GetComponent<Mover>());
			}

			if (action == "MOTION_LINEAR_TARGET")
			{
				GameObject target = Objects[args[0]];
				float speed = float.Parse(args[1]);

				Mover mover = GetOrAddComponent<Mover>(obj);
				mover.InitiateTargetMotion(obj.transform.position.x, target, speed, Mover.TrajectoryTypeE.Linear);
			}

			if (action == "MOTION_LINEAR_POS")
			{
				float pos = float.Parse(args[0]);
				float speed = float.Parse(args[1]);

				Mover mover = GetOrAddComponent<Mover>(obj);
				mover.InitiatePosMotion(obj.transform.position.x, pos, speed, Mover.TrajectoryTypeE.Linear);
			}

			if (action == "MOTION_PARABOLIC_TARGET")
			{
				GameObject target = Objects[args[0]];
				float speed = float.Parse(args[1]);

				Mover mover = GetOrAddComponent<Mover>(obj);
				mover.InitiateTargetMotion(obj.transform.position.x, target, speed, Mover.TrajectoryTypeE.Parabolic);
			}

			if (action == "MOTION_PARABOLIC_POS")
			{
				float pos = float.Parse(args[0]);
				float speed = float.Parse(args[1]);

				Mover mover = GetOrAddComponent<Mover>(obj);
				mover.InitiatePosMotion(obj.transform.position.x, pos, speed, Mover.TrajectoryTypeE.Parabolic);
			}

//			if (action == "ANIMATION_SPAWN")        // Todo ?
//			{
//
//			}

			if (action == "ANIMATION_IDLE")
			{
				SetAnimation(obj, "Idle");
			}

			if (action == "ANIMATION_WALK")
			{
				SetAnimation(obj, "Walk");
			}

			if (action == "ANIMATION_ATTACK")
			{
				SetAnimation(obj, "Attack");
			}

			if (action == "ANIMATION_DIE")
			{
				SetAnimation(obj, "Die");
			}
		}

		GloablTime += Time.deltaTime;
	}

}
