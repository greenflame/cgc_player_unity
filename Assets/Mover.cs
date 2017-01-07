using UnityEngine;
using System.Collections;
using System.IO;

public class Mover : MonoBehaviour {

    public float Speed;
    public GameObject Target;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (Target != null)
        {
            Vector3 newPosition =  transform.position;

            float d = Target.transform.position.x - transform.position.x;

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
