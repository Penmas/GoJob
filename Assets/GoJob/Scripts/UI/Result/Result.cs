using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
	[SerializeField] private GameObject resultUI;
	



	public void ResultUION()
	{

		GameManager.Instance.GameSpeed = 0;
		
		resultUI.SetActive(true);


	}

	public void ResutlUIOFF()
	{
		GameManager.Instance.GameSpeed = 1;
	}
}
