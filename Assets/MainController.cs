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

    private T GetOrAddComponent<T>(GameObject obj) where T : Component
    {
        T component = obj.GetComponent<T>();

        if (component == null)
        {
            component = obj.AddComponent<T>();
        }

        return component;
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

    private void SetMotion(GameObject obj, GameObject target, float speed)
    {
        Mover mover = GetOrAddComponent<Mover>(obj);

        mover.Target = target;
        mover.Speed = speed;
    }

    private void StopMotion(GameObject obj)
    {
        GameObject.Destroy(obj.GetComponent<Mover>());
    }

    private void SetDefaultDirection(GameObject obj)
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

    private void SetHealth(GameObject obj, int currentHealth, int maxHealth)
    {
        Health health = GetOrAddComponent<Health>(obj);

        health.MaxHealth = maxHealth;
        health.CurrentHealth = currentHealth;
    }

    private void SetCrystal(GameObject obj, string name, Crystal.Player owner)
    {
        Crystal crystal = GetOrAddComponent<Crystal>(obj);

        crystal.Name = name;
        crystal.Owner = owner;
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

                SetCrystal(obj, name, owner);
                SetDefaultDirection(obj);
            }
            else
            {
                obj = Objects[id];
            }

            SetPosition(obj, position);

            if (command[0] == "DEPLOY")
            {
                StopMotion(obj);
                SetAnimation(obj, "Idle");  // Todo
            }

            if (command[0] == "IDLE")
            {
                StopMotion(obj);
                SetAnimation(obj, "Idle");
            }

            if (command[0] == "WALK")
            {
                string targetId = command[4];
                float speed = float.Parse(command[5]);

                SetAnimation(obj, "Walk");
                SetMotion(obj, Objects[targetId], speed);
            }

            if (command[0] == "ATTACK")
            {
                string targetId = command[4];

                StopMotion(obj);
                SetAnimation(obj, "Attack");    // Todo
            }

            if (command[0] == "HEALTH")
            {
                int currentHealth = int.Parse(command[4]);
                int maxHealth = int.Parse(command[4]);

                SetHealth(obj, currentHealth, maxHealth);
            }

            if (command[0] == "DEATH")
            {
                StopMotion(obj);
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
