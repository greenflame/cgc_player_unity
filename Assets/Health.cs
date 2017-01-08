using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

    public int MaxHealth;
    public int CurrentHealth;

    private float labelDelta;

	// Use this for initialization
	void Start () {
        CurrentHealth = MaxHealth;
        Random.seed += 1;
        labelDelta = Random.Range(1, 3);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI() {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * labelDelta);
        screenPos.y = Screen.height - screenPos.y;

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.black;

        string message = string.Format("{0}/{1}", CurrentHealth, MaxHealth);
        GUI.Label( new Rect(screenPos.x, screenPos.y + 15, 200f, 100f) , message, style);
    }
}
