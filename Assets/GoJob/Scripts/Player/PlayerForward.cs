using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForward : MonoBehaviour
{
	[SerializeField] private PlayerController playerController;


	private void Update()
	{
		transform.position = playerController.transform.position + playerController.PlayerForward;
	}
}
