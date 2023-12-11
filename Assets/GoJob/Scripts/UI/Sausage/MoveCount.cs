using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCount : MonoBehaviour
{
	[SerializeField] private PlayerController playerController;
	[SerializeField] private Image gauge;


	private void Awake()
	{
		playerController = FindObjectOfType<PlayerController>();

	}

	private void Update()
	{
		gauge.fillAmount = 1 - (playerController.CurrentIdleTime / playerController.AliveTime);
	}
}
