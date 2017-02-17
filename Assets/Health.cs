using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour {

    public int MaxHealth;
    public int CurrentHealth;

	private GUIStyle BackStyle;
	private GUIStyle HealthStyle;

	private Badge Badge;

	private Color LeftPlayerColor = new Color(0f/255f, 50f/255f, 250f/255f);
	private Color RightPlayerColor = new Color(200f/255f, 50f/255f, 10f/255f);

	void Start () {
        CurrentHealth = MaxHealth;

		Badge = GetComponent<Badge>();

		BackStyle = MakeStyle(Color.gray);

		if (Badge.Owner == Player.Left)
		{
			HealthStyle = MakeStyle(LeftPlayerColor);
		}
		else
		{
			HealthStyle = MakeStyle(RightPlayerColor);
		}
	}
	
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

		if (Badge.Name == "Forge")
		{
			pos = 3.5f;
			width = 1.5f;
		}
		else if (Badge.Name == "Tower")
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
		), "", BackStyle);

		GUI.Box(new Rect(
			lu.x,
			Screen.height - lu.y,
			(rd.x - lu.x) * CurrentHealth / MaxHealth,
			lu.y - rd.y
		), "", HealthStyle);
	}
}
