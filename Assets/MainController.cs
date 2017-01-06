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

    private void SetDirectionToEnemyBase(GameObject obj)
    {
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

	void Update () {
        while (Pointer < Commands.Length && float.Parse(Commands[Pointer].Split(' ')[1]) < GloablTime)
        {
            string[] command = Commands[Pointer].Split(' ');
            Pointer++;
            Debug.Log(command[0]);

            GameObject obj = null;
            string id = command[2];
            float position = float.Parse(command[3]);

            if (command[0] == "CREATE")
            {
                // Additional params
                string name = command[4];
                Crystal.Player owner = (Crystal.Player)Enum.Parse(typeof(Crystal.Player), command[5]);

                // Create object
                obj = (GameObject)Instantiate(Resources.Load(name));
                Objects.Add(id, obj);

                // Set owner
                obj.GetComponent<Crystal>().Owner = owner;

                // Initial direction to enemy base
                SetDirectionToEnemyBase(obj);
            }
            else
            {
                obj = Objects[id];
            }

            SetPosition(obj, position);

            if (command[0] == "DEPLOY")
            {
                SetMotionTarget(obj, null);
                SetAnimation(obj, "Idle");  // Todo
            }

            if (command[0] == "IDLE")
            {
                SetMotionTarget(obj, null);
                SetAnimation(obj, "Idle");
            }

            if (command[0] == "WALK")
            {
                string targetId = command[4];

                SetAnimation(obj, "Walk");
                SetMotionTarget(obj, Objects[targetId]);
            }

            if (command[0] == "ATTACK")
            {
                string targetId = command[4];

                SetMotionTarget(obj, null);
                SetAnimation(obj, "Attack");    // Todo
            }

            if (command[0] == "HEALTH")
            {
                int health = int.Parse(command[4]);

                obj.GetComponent<Health>().CurrentHealth = health;
            }

            if (command[0] == "DEATH")
            {
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
