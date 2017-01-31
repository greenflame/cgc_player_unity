using UnityEngine;
using System.Collections;

public class Crystal : MonoBehaviour {

    public enum Player
    {
        Left,
        Right
    }

    public string Name;
    public Player Owner;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI() {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up);
        screenPos.y = Screen.height - screenPos.y;

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.black;

        string message = string.Format("{0}/{1}", Name, Owner);
        GUI.Label( new Rect(screenPos.x, screenPos.y, 200f, 100f) , message, style);
    }
}
