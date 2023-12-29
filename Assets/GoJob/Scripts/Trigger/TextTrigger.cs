using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextTrigger : MonoBehaviour
{
	[SerializeField] private bool isTextOn;
	[SerializeField] private TextMeshProUGUI text;


	private Color defaultColor;


	private void Awake()
	{
		defaultColor = text.color;
	}



	public void TextOn()
	{
		StartCoroutine("TextOnCoroutine");
	}
	public void TextOff()
	{
		StartCoroutine("TextOffCoroutine");
	}

	public IEnumerator TextOnCoroutine()
	{

		float alpha = 0f;
		text.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0);
		text.gameObject.SetActive(true);
		while (true)
		{
			alpha += Time.deltaTime;

			if(alpha > 1f)
			{
				text.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 1);

				break;
			}
			text.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, alpha);
			yield return null;

		}
		yield return null;
	}

	public IEnumerator TextOffCoroutine()
	{
		float alpha = 1f;
		text.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 1);
		text.gameObject.SetActive(true);
		while (true)
		{
			alpha -= Time.deltaTime;

			if (alpha < 0f)
			{
				text.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0);

				break;
			}
			text.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, alpha);
			yield return null;

		}
		text.gameObject.SetActive(false);
		yield return null;
	}




	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			if(isTextOn)
			{
				TextOn();
			}
			else
			{
				TextOff();
			}
		}
	}
}
