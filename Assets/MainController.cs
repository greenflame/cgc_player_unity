using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

public class MainController : MonoBehaviour
{
	private bool Enabled = true;

	public float GloablTime;
	private Dictionary<string, GameObject> Objects;
	private CommandController CommandController;

	private Text TimeText;
	private MessageRepresentor MessageRepresentor;

	private float VsMessageTime = 3;

	private PlayerController LeftController;
	private PlayerController RightController;

	void Start()
	{
		Objects = new Dictionary<string, GameObject>();
		GloablTime = 0;
		CommandController = new CommandController("game_log.txt");

		TimeText = GameObject.Find("WorldTime").GetComponent<Text>();
		MessageRepresentor = GetComponent<MessageRepresentor>();

		LeftController = GameObject.Find("LeftController").GetComponent<PlayerController>();
		RightController = GameObject.Find("RightController").GetComponent<PlayerController>();
	}

	private IEnumerator VsCoro()
	{
		Enabled = false;
		LeftController.Enabled = false;
		RightController.Enabled = false;

		MessageRepresentor.showMessage(string.Format("{0}   vs   {1}",
			LeftController.AiName, RightController.AiName), 100);
		yield return new WaitForSeconds(VsMessageTime);
		MessageRepresentor.hideMessage();

		Enabled = true;
		LeftController.Enabled = true;
		RightController.Enabled = true;
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
		if (!Enabled)
		{
			return;
		}

		GloablTime += Time.deltaTime;

		while (!CommandController.IsEnd() && float.Parse(CommandController.Top()[1]) < GloablTime)
		{
			string[] args = CommandController.Pop();

			string action = args[0];
			string id = args[2];

			args = args.Skip(3).ToArray();

			if (action == "CARDS_UPDATE")
			{
				Player player = (Player)Enum.Parse(typeof(Player), id);
				GameObject.Find(player.ToString() + "Controller")
					.GetComponent<PlayerController>().SetCards(args);
				return;
			}

			if (action == "MANA_UPDATE")
			{
				Player player = (Player)Enum.Parse(typeof(Player), id);
				GameObject.Find(player.ToString() + "Controller")
					.GetComponent<PlayerController>().SetMana(float.Parse(args[0]));
				return;
			}

			if (action == "NAME_UPDATE")
			{
				Player player = (Player)Enum.Parse(typeof(Player), id);
				GameObject.Find(player.ToString() + "Controller")
					.GetComponent<PlayerController>().SetName(args[0]);

				if (!string.IsNullOrEmpty(LeftController.AiName) && !string.IsNullOrEmpty(RightController.AiName))
				{
					StartCoroutine(VsCoro());
				}

				return;
			}

			if (action == "VERDICT_UPDATE")
			{
				Player player = (Player)Enum.Parse(typeof(Player), id);
				GameObject.Find(player.ToString() + "Controller")
					.GetComponent<PlayerController>().SetVerdict(args[0]);
				return;
			}

			if (action == "GAME_END")
			{
				// Freeze all objects
				Objects.Values.ToList().ForEach(o => {
					try
					{
						GameObject.Destroy(o.GetComponent<Mover>());
					}
					catch
					{
					}
					try
					{
						SetAnimation(o, "Idle");
					}
					catch
					{
					}
				});
					
				// Show message
				Player player = (Player)Enum.Parse(typeof(Player), id);
				string name = GameObject.Find(player.ToString() + "Controller")
					.GetComponent<PlayerController>().AiName;
				MessageRepresentor.showMessage(name + " wоn!", 100);

				// Disable controllers
				Enabled = false;
				LeftController.Enabled = false;
				RightController.Enabled = false;


				return;
			}

			if (action == "CREATE")
			{
				string name = args[0];
				Player owner = (Player)Enum.Parse(typeof(Player), args[1]);

				GameObject tmp = (GameObject)Instantiate(Resources.Load(name));
				Objects.Add(id, tmp);

				Badge badge = GetOrAddComponent<Badge>(tmp);

				badge.Name = name;
				badge.Owner = owner;

				tmp.transform.localScale = tmp.transform.localScale * 2;
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
				mover.InitiateTargetMotion(obj.transform.position, target, speed, Mover.TrajectoryTypeE.Linear);
			}

			if (action == "MOTION_LINEAR_POS")
			{
				float pos = float.Parse(args[0]);
				float speed = float.Parse(args[1]);

				Mover mover = GetOrAddComponent<Mover>(obj);
				mover.InitiatePosMotion(obj.transform.position, pos, speed, Mover.TrajectoryTypeE.Linear);
			}

			if (action == "MOTION_PARABOLIC_TARGET")
			{
				GameObject target = Objects[args[0]];
				float speed = float.Parse(args[1]);

				Mover mover = GetOrAddComponent<Mover>(obj);
				mover.InitiateTargetMotion(obj.transform.position, target, speed, Mover.TrajectoryTypeE.Parabolic);
			}

			if (action == "MOTION_PARABOLIC_POS")
			{
				float pos = float.Parse(args[0]);
				float speed = float.Parse(args[1]);

				Mover mover = GetOrAddComponent<Mover>(obj);
				mover.InitiatePosMotion(obj.transform.position, pos, speed, Mover.TrajectoryTypeE.Parabolic);
			}

			if (action == "ANIMATION_SPAWN")
			{
				SetAnimation(obj, "Idle");

				float r = UnityEngine.Random.Range(-0.5f, 0.5f);
				obj.transform.Translate(new Vector3(0, r,  -5 + r));

				obj.AddComponent<Spawn>();
			}

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

		TimeText.text = string.Format("World time:\n{0}.0\nSpeed:\n{1:F1}x", (int)Mathf.Floor(GloablTime), Time.timeScale);
	}
}
