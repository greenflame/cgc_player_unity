using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour {

    public int MaxHealth;
    public int CurrentHealth;

	// Use this for initialization
	void Start () {
        CurrentHealth = MaxHealth;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	GUIStyle MakeStyle(Color color)
	{
		Texture2D t = new Texture2D(1, 1);
		t.SetPixel(0, 0, color);
		t.Apply();

		GUIStyle s = new GUIStyle();
		s.normal.background = t;

		return s;
	}

    void OnGUI() {
		
		if (CurrentHealth == 0)
		{
			return;
		}

		float pos = 2f;
		float width = 1;
		float height = 0.1f;

		if (GetComponent<Badge>().Name == "Forge")
		{
			pos = 3.5f;
			width = 1.5f;
		}

		if (GetComponent<Badge>().Name == "Tower")
		{
			pos = 2.5f;
			width = 1.5f;
		}

		Vector3 mid = transform.position + Vector3.up * pos;

		Vector3 r = Vector3.right * width / 2;
		Vector3 u = Vector3.up * height / 2;

		Vector3 lu = Camera.main.WorldToScreenPoint(mid - r + u);
		Vector3 rd = Camera.main.WorldToScreenPoint(mid + r - u);

		GUI.Box(new Rect(
			lu.x,
			Screen.height - lu.y,
			rd.x - lu.x,
			lu.y - rd.y
		), "", MakeStyle(Color.gray));

		GUI.Box(new Rect(
			lu.x,
			Screen.height - lu.y,
			(rd.x - lu.x) * CurrentHealth / MaxHealth,
			lu.y - rd.y
		), "", MakeStyle(new Color(0.77f, 0.18f, 0.05f)));
	}
}
