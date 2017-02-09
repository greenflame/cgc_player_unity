using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float CurrentMana = 0;

	public const float ManaProductionSpeed = 1f / 2.8f;
	public const float MaxMana = 8;

	// Use this for initialization
	void Start () {

	}

	public void SetMana(float mana)
	{
		CurrentMana = mana;
	}

	public void SetCards(string[] cards)
	{
		for (int i = 0; i < cards.Length; i++)
		{
			string curTroopName = cards[i];

			if (curTroopName.EndsWith("s"))
			{
				curTroopName = curTroopName.Substring(0, curTroopName.Length - 1);
			}

			string curImageName = "card_" + i.ToString();

			Image image = transform.Find(curImageName).GetComponent<Image>();
			Sprite[] sprites = Resources.LoadAll<Sprite>(string.Format("{0}/{1}", curTroopName, curTroopName));
			image.sprite = sprites.Single(s => s.name == "i0");
		}
	}

	// Update is called once per frame
	void Update () {
		CurrentMana = Mathf.Min(MaxMana, CurrentMana + ManaProductionSpeed * Time.deltaTime);

		RectTransform manaBar = transform.Find("mana_front").GetComponent<RectTransform>();
		Vector3 scale =  manaBar.localScale;
		scale.x = 1f / MaxMana * CurrentMana;
		manaBar.localScale = scale;

		transform.Find("mana_text").GetComponent<Text>().text = string.Format("{0} / {1}", (int)Mathf.Floor(CurrentMana), (int)MaxMana);
	}
}
