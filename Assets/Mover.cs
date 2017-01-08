using UnityEngine;
using System.Collections;
using System.IO;

public class Mover : MonoBehaviour {

    public float Speed;
    public GameObject Target;
    public float PosTarget;
    public bool PosTargetEnable = false;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (!PosTargetEnable && Target == null)
        {
            return;
        }
        else
        {
            Vector3 newPosition = transform.position;
            float targetPos = PosTargetEnable ? PosTarget : Target.transform.position.x;

            float d = targetPos - transform.position.x;

            if (d == 0)
            {
                return;
            }

            newPosition.x = newPosition.x + d / Mathf.Abs(d) * Speed * Time.deltaTime;

            transform.position = newPosition;

//            transform.Translate((Target.transform.position - transform.position).normalized * Speed * Time.deltaTime);
        }
	}
}
