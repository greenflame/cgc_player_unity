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
            transform.Translate((Target.transform.position - transform.position).normalized * Speed * Time.deltaTime);
        }
	}
}
