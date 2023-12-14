using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCount : MonoBehaviour
{
	[SerializeField] private PlayerController playerController;
	[SerializeField] private Image gauge;

	private Vector3 defaultRotation;

	private void Awake()
	{
		playerController = FindObjectOfType<PlayerController>();

		defaultRotation = transform.rotation.eulerAngles;

	}

	private void Update()
	{
		gauge.fillAmount = 1 - (playerController.CurrentIdleTime / playerController.AliveTime);


		transform.eulerAngles = defaultRotation;
	}
}
