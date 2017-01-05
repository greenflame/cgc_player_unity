using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

public class MainController : MonoBehaviour {

    public float GloablTime;
    private string[] Commands;
    private int Pointer;

    private Dictionary<string, GameObject> Objects;

	// Use this for initialization
	void Start () {
        Objects = new Dictionary<string, GameObject>();

        Commands = File.ReadAllLines("/Users/Alexander/Desktop/log1");
        Pointer = 0;
        GloablTime = 0;
	}

    private void SetPosition(GameObject obj, float position)
    {
        Vector3 pos = obj.transform.position;
        pos.x = position;
        obj.transform.position = pos;
    }

    private void SetAnimation(GameObject obj, string animationName)
    {
        string animationFullName = obj.GetComponent<Crystal>().Name + animationName;
        obj.GetComponent<Animator>().Play(animationFullName, 0, 0);
    }

    private void SetMotionTarget(GameObject obj, GameObject target)
    {
        if (obj.GetComponent<Mover>() != null)
        {
            obj.GetComponent<Mover>().Target = target;
        }
        else
        {
            Debug.Log("Object hasn't mover component");
        }
    }

	// Update is called once per frame
	void Update () {
        while (Pointer < Commands.Length && float.Parse(Commands[Pointer].Split(' ')[0]) < GloablTime)
        {
            string[] command = Commands[Pointer].Split(' ');
            Pointer++;

            Debug.Log(command[1]);

            if (command[1] == "DECLARE")
            {
                // Parameters
                string id = command[2];
                string name = command[3];
                Crystal.Player owner = (Crystal.Player) Enum.Parse(typeof(Crystal.Player), command[4]);

                // Create object
                GameObject obj = (GameObject) Instantiate(Resources.Load(name));
                Objects.Add(id, obj);

                // Set owner
                obj.GetComponent<Crystal>().Owner = owner;

                // Initial direction to enemy base
                SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();

                switch (obj.GetComponent<Crystal>().Owner)
                {
                    case Crystal.Player.LeftPlayer:
                        spriteRenderer.flipX = false;
                        break;
                    case Crystal.Player.RightPlayer:
                        spriteRenderer.flipX = true;
                        break;
                }
            }

            if (command[1] == "DEPLOY")
            {
                // Parameters
                string id = command[2];
                float position = float.Parse(command[3]);

                // Find object
                GameObject obj = Objects[id];

                SetPosition(obj, position);
                SetAnimation(obj, "Die");
                SetMotionTarget(obj, null);
            }

            if (command[1] == "IDLE")
            {
                string id = command[2];
                float position = float.Parse(command[3]);

                GameObject obj = Objects[id];

                SetPosition(obj, position);
                SetAnimation(obj, "Idle");
                SetMotionTarget(obj, null);
            }

            if (command[1] == "WALK")
            {
                string id = command[2];
                float position = float.Parse(command[3]);
                string targetId = command[4];

                GameObject obj = Objects[id];

                SetPosition(obj, position);
                SetAnimation(obj, "Walk");
                SetMotionTarget(obj, Objects[targetId]);
            }

            if (command[1] == "SHOT")
            {
                string id = command[2];
                float position = float.Parse(command[3]);
                string targetId = command[4];

                GameObject obj = Objects[id];

                SetPosition(obj, position);
                SetAnimation(obj, "Attack");
                SetMotionTarget(obj, null);

                // todo
            }

            if (command[1] == "HEALTH")
            {
                string id = command[2];
//                float position = float.Parse(command[3]);
                int health = int.Parse(command[3]);

                GameObject obj = Objects[id];

//                SetPosition(obj, position);
                obj.GetComponent<Health>().CurrentHealth = health;
            }

            if (command[1] == "DEATH")
            {
                string id = command[2];

                GameObject obj = Objects[id];

//                SetPosition
                SetMotionTarget(obj, null);
                SetAnimation(obj, "Die");
                StartCoroutine(DeleteCoro(obj, 4));
            }
        }

        GloablTime += Time.deltaTime;
	}

    private IEnumerator DeleteCoro(GameObject target, float time)
    {
        yield return new WaitForSeconds(time);
        GameObject.Destroy(target);
    }
}
