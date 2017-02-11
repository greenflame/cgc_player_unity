using UnityEngine;
using System.Collections;

public class MessageRepresentor : MonoBehaviour
{

    private string messageString = "";
    private float showTimeLeft = 0;

	public Vector2 padding = new Vector2(50, 30);
	public int fontSize = 20;
	public Color backgroundColor = new Color(0.142f, 0.164f, 0.091f);
	public Color textColor = Color.white;
	public Vector2 persentTransform = new Vector2(0, 0);

    // Mechanics
    private Texture2D backgroundTexture;
    private Vector2 screenCenter;

    public void showMessage(string messageString, float showTime)
    {
        this.messageString = messageString;
        showTimeLeft = showTime;
    }

	public void hideMessage()
	{
		showTimeLeft = -1;
	}

    // Use this for initialization
    void Start()
    {
        // Class mechanics
        backgroundTexture = new Texture2D(1, 1);
        backgroundTexture.SetPixel(0, 0, backgroundColor);
        backgroundTexture.Apply();
    }

    // Update is called once per frame
    void Update()
    {
		screenCenter = new Rect(0, 0, Screen.width, Screen.height).center;
    }

    public void OnGUI()
    {
        // Message window
        if (showTimeLeft > 0)
        {
            showTimeLeft -= Time.deltaTime;

            GUIContent labelContent = new GUIContent(messageString);
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.fontSize = fontSize;
            if (labelStyle.normal != null)
            {
                labelStyle.normal.textColor = textColor;
            }
            Vector2 labelSize = labelStyle.CalcSize(labelContent);
            Rect labelRect = new Rect(screenCenter - labelSize / 2, labelSize);
            Rect backgroundRect = new Rect(labelRect.position - padding, labelRect.size + padding * 2);

			Vector2 pixelTransform = new Vector2 (screenCenter.x * persentTransform.x, screenCenter.y * persentTransform.y);

			labelRect.position += pixelTransform;
			backgroundRect.position += pixelTransform;

            GUI.DrawTexture(backgroundRect, backgroundTexture);
            GUI.Label(labelRect, labelContent, labelStyle);
        }
    }
}
