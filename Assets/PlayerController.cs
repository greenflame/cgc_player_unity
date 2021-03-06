﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public bool Enabled = true;

	public float CurrentMana = 0;

	public const float ManaProductionSpeed = 1f / 2.8f;
	public const float MaxMana = 8;

	public string AiName = "";
	private string Verdict = "";

	private RectTransform ManaBar;
	private Text ManaText;

	void Start () {
		ManaBar = transform.Find("mana_front").GetComponent<RectTransform>();
		ManaText = transform.Find("mana_text").GetComponent<Text>();
	}

	private void UpdateStrategyLabel()
	{
		transform.Find("strategy").GetComponent<Text>().text = string.Format("{0} [{1}]", AiName, Verdict);
	}

	public void SetMana(float mana)
	{
		CurrentMana = mana;
	}

	public void SetName(string name)
	{
		AiName = name;
		UpdateStrategyLabel();
	}

	public void SetVerdict(string verdict)
	{
		Verdict = verdict;
		UpdateStrategyLabel();
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

			GameObject imageObj = transform.Find(curImageName).gameObject;
			Image image = imageObj.GetComponent<Image>();
			Sprite[] sprites = Resources.LoadAll<Sprite>(string.Format("{0}/{1}", curTroopName, curTroopName));
			Sprite sprite = sprites.Single(s => s.name == "i0");
			image.sprite = sprite;

			Vector3 scale = imageObj.GetComponent<RectTransform>().localScale;
			scale.x = sprite.rect.width / sprite.rect.height;
			imageObj.GetComponent<RectTransform>().localScale = scale;
		}
	}

	void Update () {
		if (!Enabled)
		{
			return;
		}

		CurrentMana = Mathf.Min(MaxMana, CurrentMana + ManaProductionSpeed * Time.deltaTime);

		Vector3 scale =  ManaBar.localScale;
		scale.x = 1f / MaxMana * CurrentMana;
		ManaBar.localScale = scale;

		ManaText.text = string.Format("{0} / {1}", (int)Mathf.Floor(CurrentMana), (int)MaxMana);
	}
}
